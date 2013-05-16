using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaRequisicaoDeCompra
    {
        KendoGridVm Listar(PaginacaoVm paginacaoVm, RequisicaoDeCompraFiltroVm filtro);
    }
}