using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaProduto
    {
        KendoGridVm FornecedoresDoProduto(PaginacaoVm paginacaoVm, string codigoProduto);
        KendoGridVm FornecedoresDosProdutos(PaginacaoVm paginacaoVm, string[] codigoDosProdutos);
        ProdutoCadastroVm ConsultaPorCodigo(string codigoProduto);
        KendoGridVm Listar(PaginacaoVm paginacaoVm, ProdutoCadastroVm filtro);
    }
}
