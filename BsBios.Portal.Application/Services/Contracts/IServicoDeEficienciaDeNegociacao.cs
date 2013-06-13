using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IServicoDeEficienciaDeNegociacao
    {
        FornecedorVm[] ListarFornecedores(int idProcessoCotacao);
        KendoGridVm CalcularResumo(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro);
        IList<dynamic> CalcularEficienciaDoItemDoProcesso(int idProcessoCotacao, int idProcessoCotacaoItem);
    }
}