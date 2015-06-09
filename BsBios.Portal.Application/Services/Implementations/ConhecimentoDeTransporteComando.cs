using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ConhecimentoDeTransporteComando : IConhecimentoDeTransporteComando
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IConhecimentosDeTransporte _conhecimentosDeTransporte ;
        private readonly IOrdensDeTransporte _ordensDeTransporte;

        public ConhecimentoDeTransporteComando(IUnitOfWork unitOfWork, IConhecimentosDeTransporte conhecimentosDeTransporte, IOrdensDeTransporte ordensDeTransporte)
        {
            _unitOfWork = unitOfWork;
            _conhecimentosDeTransporte = conhecimentosDeTransporte;
            _ordensDeTransporte = ordensDeTransporte;
        }

        public void AtribuirOrdemDeTransporte(string chaveDoConhecimento, int idDaOrdemDeTransporte)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                ConhecimentoDeTransporte conhecimentoDeTransporte = _conhecimentosDeTransporte.ComChaveEletronica(chaveDoConhecimento).Single();
                OrdemDeTransporte ordemDeTransporte = _ordensDeTransporte.BuscaPorId(idDaOrdemDeTransporte).Single();

                conhecimentoDeTransporte.VincularComOrdem(ordemDeTransporte);

                _ordensDeTransporte.Save(ordemDeTransporte);

                _conhecimentosDeTransporte.Save(conhecimentoDeTransporte);
                
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