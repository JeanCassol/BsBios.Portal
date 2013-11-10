using BsBios.Portal.Application.DTO;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IOrdemDeTransporteService
    {
        void AtualizarOrdemDeTransporte(OrdemDeTransporteAtualizarDTO ordemDeTransporteAtualizarDTO);

    }
}