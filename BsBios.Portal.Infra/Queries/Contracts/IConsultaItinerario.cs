using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaItinerario
    {
        KendoGridVm Listar(PaginacaoVm paginacaoVm, ItinerarioFiltroVm filtro);         
    }
}