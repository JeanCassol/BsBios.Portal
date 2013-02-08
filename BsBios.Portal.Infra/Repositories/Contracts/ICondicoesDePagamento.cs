using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface ICondicoesDePagamento: ICompleteRepository<CondicaoDePagamento>
    {
        CondicaoDePagamento BuscaPeloCodigoSap(string codigoSap);
    }
}
