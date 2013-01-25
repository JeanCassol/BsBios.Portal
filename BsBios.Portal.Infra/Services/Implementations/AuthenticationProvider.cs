using System.Web.Security;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AuthenticationProvider: IAuthenticationProvider
    {
        public void Autenticar(UsuarioConectado usuarioConectado)
        {
            //Se o parâmetro createPersistentCookie for setado para true tem que criar 
            //um novo filtro de autorização, que deve levar em contato se a sessão já expirou ou não.
            FormsAuthentication.SetAuthCookie(usuarioConectado.NomeCompleto, false);    
        }

        public void Desconectar()
        {
            FormsAuthentication.SignOut();
        }
    }
}
