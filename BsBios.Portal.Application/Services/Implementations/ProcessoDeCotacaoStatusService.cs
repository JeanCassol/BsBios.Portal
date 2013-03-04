using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ProcessoDeCotacaoStatusService : IProcessoDeCotacaoStatusService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProcessosDeCotacao _processosDeCotacao;

        public ProcessoDeCotacaoStatusService(IUnitOfWork unitOfWork, IProcessosDeCotacao processosDeCotacao)
        {
            _unitOfWork = unitOfWork;
            _processosDeCotacao = processosDeCotacao;
        }


        public void AbrirProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Abrir();
                _processosDeCotacao.Save(processoDeCotacao);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.RollBack();
                throw;
            }
        }

        public void FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                ProcessoDeCotacao processoDeCotacao = _processosDeCotacao.BuscaPorId(idProcessoCotacao).Single();
                processoDeCotacao.Fechar();
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