using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class CancelamentoDeProcessoDeCotacaoService:ICancelamentoDeProcessoDeCotacaoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public CancelamentoDeProcessoDeCotacaoService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
        }

        public void Executar(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();

                processoDeCotacao.Cancelar();

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