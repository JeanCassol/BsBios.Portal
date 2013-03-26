using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IFornecedores: ICompleteRepository<Fornecedor>
    {
        Fornecedor BuscaPeloCodigo(string codigo);
        IFornecedores BuscaPeloCnpj(string cnpj);
        IFornecedores BuscaListaPorCodigo(string[] codigoDosFornecedores);
        IFornecedores FornecedoresNaoVinculadosAoProduto(string codigoProduto);
        IFornecedores NomeContendo(string filtroNome);
        IFornecedores CodigoContendo(string filtroCodigo);
        IFornecedores SomenteTransportadoras();
        IFornecedores RemoveTransportadoras();
    }
}