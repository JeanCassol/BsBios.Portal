using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{

    [SecurityFilter]
    public class ConhecimentoDeTransporteController : Controller
    {

        private readonly IConsultaConhecimentoDeTransporte _consulta;

        public ConhecimentoDeTransporteController(IConsultaConhecimentoDeTransporte consulta)
        {
            _consulta = consulta;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar(PaginacaoVm paginacao, FiltroDeConhecimentoDeTransporte filtro)
        {
            KendoGridVm kendoGridVm = _consulta.Listar(paginacao, filtro);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
