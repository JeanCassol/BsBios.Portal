using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IItinerarios: ICompleteRepository<Itinerario>
    {
        IItinerarios BuscaPeloCodigo(string codigo);
    }
}
