using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Usuarios: CompleteRepositoryNh<Usuario>, IUsuarios
    {
        public Usuarios(IUnitOfWorkNh unitOfWorkNh) : base(unitOfWorkNh)
        {
        }

        public Usuario BuscaPorId(int idUsuario)
        {
            return Query.SingleOrDefault(u => u.Id == idUsuario);
        }

        public Usuario BuscaPorLogin(string login)
        {
            return Query.SingleOrDefault(u => u.Login == login);
        }
    }
}
