using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaRequisicaoDeCompra
    {
        KendoGridVm Listar(PaginacaoVm paginacaoVm, RequisicaoDeCompraFiltroVm filtro);
        IList<RequisicaoDeCompraVm> RequisicoesDoGrupoDeCompras(string codigoDoGrupoDeCompras);
    }
}