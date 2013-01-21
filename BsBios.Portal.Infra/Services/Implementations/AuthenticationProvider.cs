using System.Web.Security;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AuthenticationProvider: IAuthenticationProvider
    {
        public void Autenticar(UsuarioConectado usuarioConectado)
        {
            FormsAuthentication.SetAuthCookie(usuarioConectado.NomeCompleto, true);    
        }
    }
}
