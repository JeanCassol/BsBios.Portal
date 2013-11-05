using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;

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

        protected void ExecutarServicosDeFechamento(ProcessoDeCotacao processoDeCotacao)
        {
            _comunicacaoSap.EfetuarComunicacao(processoDeCotacao);
            _geradorDeEmail.GerarEmail(processoDeCotacao);
            ProcessosDeCotacao.Save(processoDeCotacao);
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
                processoDeCotacao.Fechar();
                ExecutarServicosDeFechamento(processoDeCotacao);

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
                IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso();

                foreach (var ordemDeTransporte in ordensDeTransporte)
                {
                    _ordensDeTransporte.Save(ordemDeTransporte);
                }

                ExecutarServicosDeFechamento(processoDeCotacao);

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
