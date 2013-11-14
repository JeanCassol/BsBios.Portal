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
        private readonly IConsultaParaConferenciaDeCargas _consultaParaConferenciaDeCargas;

        public AgendamentoDeCargaController(IConsultaQuota consultaQuota, IConsultaParaConferenciaDeCargas consultaParaConferenciaDeCargas)
        {
            _consultaQuota = consultaQuota;
            _consultaParaConferenciaDeCargas = consultaParaConferenciaDeCargas;
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
            AgendamentoDeCargaCadastroVm model = null;
            if (codigoFluxoDeCarga == Enumeradores.FluxoDeCarga.Carregamento)
            {
                model = new AgendamentoDeCarregamentoCadastroVm() { IdQuota = idQuota };
            }
            if (codigoFluxoDeCarga == Enumeradores.FluxoDeCarga.Descarregamento)
            {
                model = new AgendamentoDeDescarregamentoCadastroVm() {IdQuota = idQuota};
            }

            if (model != null)
            {
                model.PermiteEditar = true;
                model.PermiteRealizar = false;
            }
            return PartialView(ViewDoFluxoDeCarga(codigoFluxoDeCarga), model);

        }

        public ActionResult EditarCadastro(int idQuota, int idAgendamento)
        {
            AgendamentoDeCargaCadastroVm cadastroVm = _consultaQuota.ConsultarAgendamento(idQuota, idAgendamento);
            //cadastroVm.PermiteEditar = true;
            return PartialView(cadastroVm.ViewDeCadastro, cadastroVm);
        }

        public ActionResult RealizarAgendamento(int idQuota, int idAgendamento)
        {
            AgendamentoDeCargaCadastroVm cadastroVm = _consultaQuota.ConsultarAgendamento(idQuota, idAgendamento);
            //cadastroVm.PermiteEditar = false;
            return PartialView(cadastroVm.ViewDeCadastro, cadastroVm);
        }


        [HttpGet]
        public JsonResult Consultar(PaginacaoVm paginacaoVm, ConferenciaDeCargaFiltroVm filtro)
        {
            KendoGridVm kendoGridVm = _consultaParaConferenciaDeCargas.Consultar(paginacaoVm, filtro);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}