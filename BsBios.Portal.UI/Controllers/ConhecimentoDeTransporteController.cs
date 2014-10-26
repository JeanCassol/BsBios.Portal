using System.Web.Mvc;
using BsBios.Portal.Application.DTO;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{

    [SecurityFilter]
    public class ConhecimentoDeTransporteController : BaseController
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

        public ActionResult Formulario(string chaveEletronica)
        {
            ConhecimentoDeTransporteFormulario conhecimentoDeTransporte = _consulta.ObterRegistro(chaveEletronica);
            return View(conhecimentoDeTransporte);
        }

        public ActionResult ListarNotasFiscais(string chaveEletronica)
        {
            KendoGridVm kendoGridVm  = _consulta.ListarNotasFiscais(chaveEletronica);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarOrdensDeTransporte(string chaveEletronica)
        {
            KendoGridVm kendoGridVm = _consulta.ListarOrdensDeTransporte(chaveEletronica);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

    }
}
