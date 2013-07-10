using System.Web.Mvc;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class EficienciaDeNegociacaoController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

    }
}
