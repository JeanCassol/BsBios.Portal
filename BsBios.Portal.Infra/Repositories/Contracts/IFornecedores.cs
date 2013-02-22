using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IFornecedores: ICompleteRepository<Fornecedor>
    {
        Fornecedor BuscaPeloCodigo(string codigo);
        IFornecedores BuscaListaPorCodigo(string[] codigoDosFornecedores);
    }
}