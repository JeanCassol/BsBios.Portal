using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BsBios.Portal.UI.Controllers
{
    public class CotacaoController : Controller
    {
        [HttpGet]
        public ActionResult EditarCadastro()
        {
            return View("Cadastro");
        }

    }
}
