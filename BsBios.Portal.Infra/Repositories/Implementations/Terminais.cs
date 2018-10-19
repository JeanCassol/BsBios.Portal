using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Terminais: CompleteRepositoryNh<Terminal>, ITerminais
    {
        public Terminais(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public Terminal BuscaPeloCodigo(string codigo)
        {
            return Query.SingleOrDefault(x => x.Codigo == codigo);
        }

        public ITerminais BuscaListaPorCodigo(string[] codigos)
        {
            Query = Query.Where(terminal => codigos.Contains(terminal.Codigo));
            return this;
        }
    }
}
