using System.Web.Mvc;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ConferenciaDeCargaController : Controller
    {
        //
        // GET: /ConferenciaDeCarga/

        public ActionResult Pesquisar()
        {
            return View();
        }

    }
}
