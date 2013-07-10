using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RequisicaoDeCompraController : Controller
    {
        private readonly IConsultaRequisicaoDeCompra _consulta;

        public RequisicaoDeCompraController(IConsultaRequisicaoDeCompra consulta)
        {
            _consulta = consulta;
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, RequisicaoDeCompraFiltroVm filtro)
        {
            KendoGridVm kendoGridVm = _consulta.Listar(paginacaoVm, filtro);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}