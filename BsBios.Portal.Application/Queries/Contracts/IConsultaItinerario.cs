using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaItinerario
    {
        KendoGridVm Listar(PaginacaoVm paginacaoVm, ItinerarioFiltroVm filtro);         
    }
}