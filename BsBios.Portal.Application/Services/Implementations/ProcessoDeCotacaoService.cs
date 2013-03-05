using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoService : IProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ProcessoDeCotacaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
        }


        public void AtualizarProcesso(ProcessoDeCotacaoAtualizarVm atualizacaoDoProcessoDeCotacaoVm)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                var processoDeCotacao = _processosDeCotacao.BuscaPorId(atualizacaoDoProcessoDeCotacaoVm.Id).Single();
                processoDeCotacao.Atualizar(atualizacaoDoProcessoDeCotacaoVm.DataLimiteRetorno);
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