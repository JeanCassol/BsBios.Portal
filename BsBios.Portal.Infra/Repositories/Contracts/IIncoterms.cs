using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IIncoterms: ICompleteRepository<Incoterm>
    {
        IIncoterms BuscaPeloCodigo(string codigo);
    }
}