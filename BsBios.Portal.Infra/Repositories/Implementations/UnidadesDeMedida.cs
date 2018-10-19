using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class UnidadesDeMedida:CompleteRepositoryNh<UnidadeDeMedida>, IUnidadesDeMedida
    {
        public UnidadesDeMedida(IUnitOfWorkNh unitOfWork)
            : base(unitOfWork)
        {
        }

        public IUnidadesDeMedida BuscaPeloCodigoInterno(string codigoInterno)
        {
            Query = Query.Where(x => x.CodigoInterno == codigoInterno);
            return this;
        }

        public IUnidadesDeMedida FiltraPorListaDeCodigosInternos(string[] codigos)
        {
            Query = Query.Where(x => codigos.Contains(x.CodigoInterno));
            return this;
        }
    }
}
