using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUsuarios: ICompleteRepository<Usuario>
    {
        Usuario BuscaPorLogin(string login);
        IUsuarios NomeContendo(string filtroNome);
        IUsuarios LoginContendo(string login);
        IUsuarios FiltraPorListaDeLogins(string[] logins);
        Usuario UsuarioConectado();
        IUsuarios SemSenha();
        IUsuarios ContendoPerfil(Enumeradores.Perfil perfil);
    }
}