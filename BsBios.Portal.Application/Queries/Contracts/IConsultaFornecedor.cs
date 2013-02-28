using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaFornecedor
    {
        KendoGridVm FornecedoresNaoVinculadosAoProduto(string codigoProduto);
    }
}