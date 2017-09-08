using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class FechamentoDeProcessoDeCotacaoDeMaterialService : IFechamentoDeProcessoDeCotacaoDeMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;
        private readonly IProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap _comunicacaoSap;

        public FechamentoDeProcessoDeCotacaoDeMaterialService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao,
            IProcessoDeCotacaoDeMaterialFechamentoComunicacaoSap comunicacaoSap)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
            _comunicacaoSap = comunicacaoSap;
        }

        public void Executar(ProcessoDeCotacaoDeMaterialFechamentoInfoVm processoDeCotacaoFechamentoVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var processoDeCotacao = (ProcessoDeCotacaoDeMaterial)_processosDeCotacao.BuscaPorId(processoDeCotacaoFechamentoVm.IdProcessoCotacao).Single();
                processoDeCotacao.Fechar();
                //TODO COMENTADO PARA APRESENTACAO
                //_comunicacaoSap.EfetuarComunicacao(processoDeCotacao, processoDeCotacaoFechamentoVm);
                _processosDeCotacao.Save(processoDeCotacao);

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
