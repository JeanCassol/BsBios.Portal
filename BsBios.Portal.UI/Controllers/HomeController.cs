using System;
using System.Web.Mvc;
using BsBios.Portal.Infra.Builders;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    public class HomeController : Controller
    {
        [SecurityFilter]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            try
            {
                var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
                ViewBag.UsuarioConectado = usuarioConectado;
                var menuUsuarioBuilder = new MenuUsuarioBuilder(usuarioConectado.Perfis);
                return View("_Menu", menuUsuarioBuilder.Construct());

            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
