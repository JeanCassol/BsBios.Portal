using System.Web.Security;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AuthenticationProvider: IAuthenticationProvider
    {
        public UsuarioConectado Autenticar(string usuario, string senha)
        {
            usuario = usuario.ToLower();
            UsuarioConectado usuarioConectado;
            if (usuario == "comprador" && senha == "123")
            {
                usuarioConectado = new UsuarioConectado(usuario,new PerfilComprador());
            }
            else if (usuario == "fornecedor" && senha == "123")
            {
                usuarioConectado = new UsuarioConectado(usuario, new PerfilFornecedor());
            }
            else
            {
                usuarioConectado = new UsuarioConectado(usuario,new PerfilNaoAutorizado());
            }
            if (usuarioConectado.Perfil.PermiteLogin)
            {
                FormsAuthentication.SetAuthCookie(usuario, true);    
            }

            return usuarioConectado;
        }
    }
}
