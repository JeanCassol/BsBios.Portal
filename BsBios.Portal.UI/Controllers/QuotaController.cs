using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class QuotaController : Controller
    {
        private readonly IConsultaQuota _consultaQuota;

        public QuotaController(IConsultaQuota consultaQuota)
        {
            _consultaQuota = consultaQuota;
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Pesquisar(DateTime dataDaQuota)
        {
            try
            {
                return Json(new{ Sucesso = true, TemQuota = _consultaQuota.PossuiQuotaNaData(dataDaQuota)}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
