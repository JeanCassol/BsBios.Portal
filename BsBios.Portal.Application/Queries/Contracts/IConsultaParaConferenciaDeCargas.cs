using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{

    public interface IConsultaParaConferenciaDeCargas
    {
        KendoGridVm Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro);
         
    }
}