using System.Web;
using System.Web.Security;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class AuthenticationProvider: IAuthenticationProvider
    {
        public void Autenticar(UsuarioConectado usuarioConectado)
        {
            //if (HttpContext.Current.User.Identity.IsAuthenticated)
            //{
            //    Desconectar();
            //}
            //Se o parâmetro createPersistentCookie for setado para true tem que criar 
            //um novo filtro de autorização, que deve levar em contato se a sessão já expirou ou não.
            FormsAuthentication.SetAuthCookie(usuarioConectado.NomeCompleto, false);
            HttpContext.Current.Session["UsuarioConectado"] = usuarioConectado;
            ObjectFactory.Configure(c => c.For<UsuarioConectado>()
            .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.HybridHttpSession))
            .Use(() => (UsuarioConectado) HttpContext.Current.Session["UsuarioConectado"])
    );

        }

        public void Desconectar()
        {
            FormsAuthentication.SignOut();
        }
    }
}
