using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class PerfilController : Controller
    {
        private readonly IConsultaPerfil _consultaPerfil;

        public PerfilController(IConsultaPerfil consultaPerfil)
        {
            _consultaPerfil = consultaPerfil;
        }

        [HttpGet]
        public ActionResult Listar()
        {
            try
            {
                IList<PerfilVm> perfis = _consultaPerfil.Listar();
                return Json(new {Sucesso = true, Registros = perfis}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}