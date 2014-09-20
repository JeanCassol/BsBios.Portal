using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IIvas: ICompleteRepository<Iva>
    {
        Iva BuscaPeloCodigo(string codigo);
        IIvas BuscaListaPorCodigo(string[] codigosIva);
    }
}
