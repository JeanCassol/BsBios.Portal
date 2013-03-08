using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IIvas: ICompleteRepository<Iva>
    {
        Iva BuscaPeloCodigo(string codigoSap);
        IIvas BuscaListaPorCodigo(string[] codigosIva);
    }
}
