using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Itinerarios:CompleteRepositoryNh<Itinerario>, IItinerarios
    {
        public Itinerarios(IUnitOfWorkNh unitOfWork)
            : base(unitOfWork)
        {
        }

        public IItinerarios BuscaPeloCodigo(string codigo)
        {
            Query = Query.Where(x => x.Codigo == codigo);
            return this;
        }

        public IItinerarios FiltraPorListaDeCodigos(string[] codigos)
        {
            Query = Query.Where(x => codigos.Contains(x.Codigo));
            return this;
        }
    }
}
