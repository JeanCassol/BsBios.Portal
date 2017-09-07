using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Model;
using NHibernate.Linq;
using StructureMap;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Usuarios: CompleteRepositoryNh<Usuario>, IUsuarios
    {
        public Usuarios(IUnitOfWorkNh unitOfWorkNh) : base(unitOfWorkNh)
        {
            Query = Query.OrderBy(x => x.Nome);
        }

        public Usuario BuscaPorLogin(string login)
        {
            return Query.SingleOrDefault(u => u.Login.ToLower() == login.Trim().ToLower());
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
                Query = Query.Where(x => x.Login.ToLower().Contains(login.ToLower()));
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

        public IUsuarios SemSenha()
        {
            Query = Query.Where(x => x.Senha == null);
            return this;
        }

        public IUsuarios EmailContendo(string email)
        {
            if (!string.IsNullOrEmpty(email))
            {
                Query = Query.Where(x => x.Email.ToLower().Contains(email.ToLower()));
            }

            return this;
        }

        public IUsuarios ComStatus(Enumeradores.StatusUsuario status)
        {
            Query = Query.Where(x => x.Status == status);

            return this;
        }

        public IUsuarios IncluirPerfis()
        {
            Query = Query.FetchMany(usuario => usuario.Perfis);

            return this;
        }
        public IUsuarios ContendoPerfil(Enumeradores.Perfil perfil)
        {
            Query = Query.Where(x => x.Perfis.Contains(perfil));
            return this;
        }

    }
}
