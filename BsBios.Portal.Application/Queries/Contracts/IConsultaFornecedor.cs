using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaFornecedor
    {
        KendoGridVm FornecedoresNaoVinculadosAoProduto(PaginacaoVm paginacaoVm, FornecedorDoProdutoFiltro filtro);
        KendoGridVm Listar(PaginacaoVm paginacaoVm, FornecedorFiltroVm filtro);
        FornecedorCadastroVm ConsultaPorCodigo(string codigoDoFornecedor);
        KendoGridVm ProdutosDoFornecedor(PaginacaoVm paginacaoVm, string codigoFornecedor);
        string ConsultaPorCnpj(string cnpj);
    }
}