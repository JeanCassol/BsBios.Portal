using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IIncoterms: ICompleteRepository<Incoterm>
    {
        IIncoterms BuscaPeloCodigo(string codigo);
        IIncoterms FiltraPorListaDeCodigos(string[] codigos);
    }
}