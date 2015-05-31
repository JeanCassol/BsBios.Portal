using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeUsuarioController : BaseController
    {
        public ActionResult Index()
        {
            return View("Relatorio");
        }

    }
}
