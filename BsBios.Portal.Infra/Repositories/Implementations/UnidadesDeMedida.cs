using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

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

    }
}
