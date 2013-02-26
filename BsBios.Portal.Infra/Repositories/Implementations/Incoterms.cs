using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Incoterms: CompleteRepositoryNh<Incoterm>, IIncoterms
    {
        public Incoterms(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IIncoterms BuscaPeloCodigo(string codigo)
        {
            Query = Query.Where(x => x.Codigo == codigo);
            return this;
        }
    }
}
