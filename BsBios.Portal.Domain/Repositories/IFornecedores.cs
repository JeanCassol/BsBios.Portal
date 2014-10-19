using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IFornecedores: ICompleteRepository<Fornecedor>
    {
        Fornecedor BuscaPeloCodigo(string codigo);
        Fornecedor BuscaPeloCnpj(string cnpj);
        IFornecedores BuscaListaPorCodigo(string[] codigoDosFornecedores);
        IFornecedores FornecedoresNaoVinculadosAoProduto(string codigoProduto);
        IFornecedores NomeContendo(string filtroNome);
        IFornecedores CodigoContendo(string filtroCodigo);
        IFornecedores SomenteTransportadoras();
        IFornecedores RemoveTransportadoras();
    }
}