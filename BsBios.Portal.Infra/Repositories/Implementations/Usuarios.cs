using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Usuarios: CompleteRepositoryNh<Usuario>, IUsuarios
    {
        public Usuarios(IUnitOfWorkNh unitOfWorkNh) : base(unitOfWorkNh)
        {
        }

        public Usuario BuscaPorLogin(string login)
        {
            return Query.SingleOrDefault(u => u.Login.ToLower() == login.ToLower());
        }

        public IUsuarios FiltraPorNome(string filtroNome)
        {
            Query = Query.Where(x => x.Nome.ToLower().Contains(filtroNome.ToLower()));
            return this;
        }
    }
}
