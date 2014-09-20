using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface ICondicoesDePagamento: ICompleteRepository<CondicaoDePagamento>
    {
        CondicaoDePagamento BuscaPeloCodigo(string codigoSap);
        ICondicoesDePagamento FiltraPelaDescricao(string descricao);
        ICondicoesDePagamento FiltraPorListaDeCodigos(string[] codigos);
    }
}
