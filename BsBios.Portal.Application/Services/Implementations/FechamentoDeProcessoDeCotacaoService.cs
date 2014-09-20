using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public abstract class FechamentoDeProcessoDeCotacao
    {
        protected readonly IProcessosDeCotacao ProcessosDeCotacao;
        private readonly IGeradorDeEmailDeFechamentoDeProcessoDeCotacao _geradorDeEmail;
        private readonly IComunicacaoSap _comunicacaoSap;

        protected FechamentoDeProcessoDeCotacao(IProcessosDeCotacao processosDeCotacao,
            IGeradorDeEmailDeFechamentoDeProcessoDeCotacao geradorDeEmail, 
            IComunicacaoSap comunicacaoSap)
        {
            ProcessosDeCotacao = processosDeCotacao;
            _geradorDeEmail = geradorDeEmail;
            _comunicacaoSap = comunicacaoSap;
        }

        protected ApiResponseMessage ExecutarServicosDeFechamento(ProcessoDeCotacao processoDeCotacao)
        {
            ApiResponseMessage apiResponseMessage = _comunicacaoSap.EfetuarComunicacao(processoDeCotacao);
            _geradorDeEmail.GerarEmail(processoDeCotacao);
            return apiResponseMessage;
        }
    }

    public class FechamentoDeProcessoDeCotacaoDeMaterialService : FechamentoDeProcessoDeCotacao, IFechamentoDeProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FechamentoDeProcessoDeCotacaoDeMaterialService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,
            IGeradorDeEmailDeFechamentoDeProcessoDeCotacao geradorDeEmail, 
            IComunicacaoSap comunicacaoSap):base(processosDeCotacao, geradorDeEmail, comunicacaoSap)
        {
            _unitOfWork = unitOfWork;
        }

        public void Executar(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                
                ProcessoDeCotacao processoDeCotacao = ProcessosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.FecharProcesso();
                ExecutarServicosDeFechamento(processoDeCotacao);
                ProcessosDeCotacao.Save(processoDeCotacao);

                _unitOfWork.Commit();

            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }
    }

    public class FechamentoDeProcessoDeCotacaoDeFreteService : FechamentoDeProcessoDeCotacao, IFechamentoDeProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrdensDeTransporte _ordensDeTransporte;

        public FechamentoDeProcessoDeCotacaoDeFreteService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,
            IGeradorDeEmailDeFechamentoDeProcessoDeCotacao geradorDeEmail,
            IComunicacaoSap comunicacaoSap, IOrdensDeTransporte ordensDeTransporte)
            : base(processosDeCotacao, geradorDeEmail, comunicacaoSap)
        {
            _unitOfWork = unitOfWork;
            _ordensDeTransporte = ordensDeTransporte;
        }

        public void Executar(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var processoDeCotacao = (ProcessoDeCotacaoDeFrete)ProcessosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

                var retorno = (ProcessoDeCotacaoDeFreteFechamentoRetorno) ExecutarServicosDeFechamento(processoDeCotacao);

                IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = retorno.Condicoes.Select(x => new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor = x.CodigoDoFornecedor,
                    NumeroGeradoNoSap = x.Numero
                });

                IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso(condicoesDeFechamento);

                ProcessosDeCotacao.Save(processoDeCotacao);

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
