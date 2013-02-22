using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProdutos : ICompleteRepository<Produto>
    {
        Produto BuscaPeloCodigo(string codigoSap);
    }
}