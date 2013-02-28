using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaProduto
    {
        KendoGridVm FornecedoresDoProduto(string codigoProduto);

    }
}
