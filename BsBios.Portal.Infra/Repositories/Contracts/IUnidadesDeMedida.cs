using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUnidadesDeMedida: ICompleteRepository<UnidadeDeMedida>
    {
        IUnidadesDeMedida BuscaPeloCodigoInterno(string codigoInterno);
        IUnidadesDeMedida FiltraPorListaDeCodigosInternos(string[] codigos);
    }
}
