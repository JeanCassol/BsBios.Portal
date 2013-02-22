using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface ICondicoesDePagamento: ICompleteRepository<CondicaoDePagamento>
    {
        CondicaoDePagamento BuscaPeloCodigoSap(string codigoSap);
        ICondicoesDePagamento FiltraPelaDescricao(string descricao);
    }
}
