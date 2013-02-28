using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaFornecedor
    {
        KendoGridVm FornecedoresDoProduto(string codigoProduto);
        KendoGridVm FornecedoresNaoVinculadosAoProduto(string codigoProduto);
    }
}