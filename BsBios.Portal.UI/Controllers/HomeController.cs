using System.Web.Mvc;
using BsBios.Portal.Infra.Builders;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ViewResult Menu()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            var menuUsuarioBuilder = new MenuUsuarioBuilder(usuarioConectado.Perfil);
            return View("_Menu", menuUsuarioBuilder.Construct());
        }
    }
}
