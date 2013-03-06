using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface ICondicoesDePagamento: ICompleteRepository<CondicaoDePagamento>
    {
        CondicaoDePagamento BuscaPeloCodigo(string codigoSap);
        ICondicoesDePagamento FiltraPelaDescricao(string descricao);
    }
}
