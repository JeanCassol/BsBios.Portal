using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IProdutos : ICompleteRepository<Produto>
    {
        Produto BuscaPeloCodigo(string codigoSap);
        IProdutos FiltraPorListaDeCodigos(string[] codigos);
        IProdutos DescricaoContendo(string filtroDescricao);
        IProdutos CodigoContendo(string codigo);
        IProdutos TipoContendo(string tipo);
    }
}