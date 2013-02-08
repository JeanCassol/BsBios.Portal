using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IIvas: ICompleteRepository<Iva>
    {
        Iva BuscaPeloCodigoSap(string codigoSap);
    }
}
