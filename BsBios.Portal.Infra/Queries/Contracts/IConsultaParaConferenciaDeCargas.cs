using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{

    public interface IConsultaParaConferenciaDeCargas
    {
        KendoGridVm Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro);
         
    }
}