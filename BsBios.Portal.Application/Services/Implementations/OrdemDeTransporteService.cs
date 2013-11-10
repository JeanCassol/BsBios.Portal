using System;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class OrdemDeTransporteService : IOrdemDeTransporteService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IOrdensDeTransporte _ordensDeTransporte;

        public OrdemDeTransporteService(IUnitOfWork unitOfWork, IOrdensDeTransporte ordensDeTransporte)
        {
            _unitOfWork = unitOfWork;
            _ordensDeTransporte = ordensDeTransporte;
        }

        public void AtualizarOrdemDeTransporte(OrdemDeTransporteAtualizarDTO ordemDeTransporteAtualizarDTO)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(ordemDeTransporteAtualizarDTO.Id).Single();
                ordemDeTransporte.AlterarQuantidadeLiberada(ordemDeTransporteAtualizarDTO.QuantidadeLiberada);
                _ordensDeTransporte.Save(ordemDeTransporte);
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