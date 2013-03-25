using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class AgendamentoDeCargaController : Controller
    {
        private readonly IConsultaQuota _consultaQuota;

        public AgendamentoDeCargaController(IConsultaQuota consultaQuota)
        {
            _consultaQuota = consultaQuota;
        }

        [HttpGet]
        public ActionResult AgendamentosDaQuota(int idQuota)
        {
            QuotaPorFornecedorVm model = _consultaQuota.ConsultarQuota(idQuota);
            return View(model);
        }


        public JsonResult ListarPorQuota(int idQuota)
        {
            KendoGridVm kendoGridVm = _consultaQuota.ListarAgendamentosDaQuota(idQuota);
            return Json(kendoGridVm,JsonRequestBehavior.AllowGet);
        }

        private string ViewDoFluxoDeCarga(Enumeradores.FluxoDeCarga materialDeCarga)
        {
            if (materialDeCarga == Enumeradores.FluxoDeCarga.Carregamento)
            {
                return "AgendamentoDeCarregamento";
            }
            if (materialDeCarga == Enumeradores.FluxoDeCarga.Descarregamento)
            {
                return "AgendamentoDeDescarregamento";
            }
            return "";
        }


        public ActionResult NovoCadastro(int idQuota, Enumeradores.FluxoDeCarga codigoFluxoDeCarga)
        {
            var model = new AgendamentoDeCarregamentoCadastroVm() {IdQuota = idQuota};
            return PartialView(ViewDoFluxoDeCarga(codigoFluxoDeCarga),model);
        }

        public ActionResult EditarCadastro(int idQuota)
        {
            throw new NotImplementedException();
        }
    }
}