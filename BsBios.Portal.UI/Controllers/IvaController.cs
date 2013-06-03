using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class IvaController : Controller
    {
        private readonly IConsultaIva _consultaIva;

        public IvaController(IConsultaIva consultaIva)
        {
            _consultaIva = consultaIva;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            try
            {
                IList<IvaCadastroVm> ivas = _consultaIva.ListarTodos();
                return Json(new {Sucesso = true, Registros = ivas }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}