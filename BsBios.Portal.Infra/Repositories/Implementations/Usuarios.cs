using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using StructureMap;

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

        public IUsuarios NomeContendo(string filtroNome)
        {
            if (!string.IsNullOrEmpty(filtroNome))
            {
                Query = Query.Where(x => x.Nome.ToLower().Contains(filtroNome.ToLower()));
            }
            return this;
        }

        public IUsuarios LoginContendo(string login)
        {
            if (!string.IsNullOrEmpty(login))
            {
                Query = Query.Where(x => x.Login.ToLower() == login.ToLower());
            }

            return this;
        }

        public IUsuarios FiltraPorListaDeLogins(string[] logins)
        {
            Query = Query.Where(x => logins.Contains(x.Login));
            return this;
        }

        public Usuario UsuarioConectado()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            return BuscaPorLogin(usuarioConectado.Login);
        }
    }
}
