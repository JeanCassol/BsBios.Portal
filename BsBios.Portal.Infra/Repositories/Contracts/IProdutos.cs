using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProdutos : ICompleteRepository<Produto>
    {
        Produto BuscaPorCodigoSap(string codigoSap);
    }
}