using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IUnidadesDeMedida: ICompleteRepository<UnidadeDeMedida>
    {
        IUnidadesDeMedida BuscaPeloCodigoInterno(string codigoInterno);
        IUnidadesDeMedida FiltraPorListaDeCodigosInternos(string[] codigos);
    }
}
