using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IUsuarios: ICompleteRepository<Usuario>
    {
        Usuario BuscaPorLogin(string login);
        IUsuarios NomeContendo(string filtroNome);
        IUsuarios LoginContendo(string login);
        IUsuarios FiltraPorListaDeLogins(string[] logins);
        Usuario UsuarioConectado();
        IUsuarios SemSenha();
        IUsuarios EmailContendo(string email);
        IUsuarios ComStatus(Enumeradores.StatusUsuario status);
        IUsuarios IncluirPerfis();
        IUsuarios ContendoPerfil(Enumeradores.Perfil perfil);

    }
}