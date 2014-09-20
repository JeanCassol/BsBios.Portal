using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
