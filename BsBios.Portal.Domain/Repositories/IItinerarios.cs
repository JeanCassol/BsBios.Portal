using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IItinerarios: ICompleteRepository<Itinerario>
    {
        IItinerarios BuscaPeloCodigo(string codigo);
        IItinerarios FiltraPorListaDeCodigos(string[] codigos);
        IItinerarios CodigoContendo(string codigo);
        IItinerarios DescricaoContendo(string descricao);
    }
}
