using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaFornecedor
    {
        KendoGridVm FornecedoresNaoVinculadosAoProduto(PaginacaoVm paginacaoVm, FornecedorDoProdutoFiltro filtro);
        KendoGridVm Listar(PaginacaoVm paginacaoVm, string filtroNome);
        FornecedorCadastroVm ConsultaPorCodigo(string codigoDoFornecedor);
        KendoGridVm ProdutosDoFornecedor(string codigoFornecedor);
    }
}