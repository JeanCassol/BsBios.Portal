using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Implementations;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    //public abstract class FechamentoDeProcessoDeCotacao
    //{
    //    protected readonly IProcessosDeCotacao ProcessosDeCotacao;
    //    private readonly IGeradorDeEmailDeFechamentoDeProcessoDeCotacao _geradorDeEmail;
    //    private readonly IProcessoDeCotacaoComunicacaoSap _comunicacaoSap;

    //    protected FechamentoDeProcessoDeCotacao(IProcessosDeCotacao processosDeCotacao,
    //        IGeradorDeEmailDeFechamentoDeProcessoDeCotacao geradorDeEmail, 
    //        IComunicacaoSap comunicacaoSap)
    //    {
    //        ProcessosDeCotacao = processosDeCotacao;
    //        _geradorDeEmail = geradorDeEmail;
    //        _comunicacaoSap = comunicacaoSap;
    //    }

    //    protected ApiResponseMessage ExecutarServicosDeFechamento(ProcessoDeCotacao processoDeCotacao)
    //    {
    //        ApiResponseMessage apiResponseMessage = _comunicacaoSap.EfetuarComunicacao(processoDeCotacao);
    //        _geradorDeEmail.GerarEmail(processoDeCotacao);
    //        return apiResponseMessage;
    //    }
    //}

    public class FechamentoDeProcessoDeCotacaoDeFreteService : IFechamentoDeProcessoDeCotacaoDeFreteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IGeradorDeEmailDeFechamentoDeProcessoDeCotacao _geradorDeEmail;
        private readonly IProcessoDeCotacaoDeFreteFechamentoComunicacaoSap _comunicacaoSap;
        private readonly IOrdensDeTransporte _ordensDeTransporte;

        public FechamentoDeProcessoDeCotacaoDeFreteService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,
            IGeradorDeEmailDeFechamentoDeProcessoDeCotacao geradorDeEmail,
            IProcessoDeCotacaoDeFreteFechamentoComunicacaoSap comunicacaoSap, IOrdensDeTransporte ordensDeTransporte)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _geradorDeEmail = geradorDeEmail;
            _comunicacaoSap = comunicacaoSap;
            _ordensDeTransporte = ordensDeTransporte;
        }

        public void Executar(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var processoDeCotacao = (ProcessoDeCotacaoDeFrete) _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

                var retorno = (ProcessoDeCotacaoDeFreteFechamentoRetorno) _comunicacaoSap.EfetuarComunicacao(processoDeCotacao);

                _geradorDeEmail.GerarEmail(processoDeCotacao);

                IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = retorno.Condicoes.Select(x => new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor = x.CodigoDoFornecedor,
                    NumeroGeradoNoSap = x.Numero
                });

                IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso(condicoesDeFechamento);

                _processosDeCotacao.Save(processoDeCotacao);

                foreach (var ordemDeTransporte in ordensDeTransporte)
                {
                    _ordensDeTransporte.Save(ordemDeTransporte);
                }


                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }
}
