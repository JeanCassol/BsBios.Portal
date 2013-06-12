using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaEficienciaDeNegociacao
    {
        KendoGridVm ConsultarResumo(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro);
        FornecedorVm[] ListarFornecedores(int idProcessoCotacao);
    }
}