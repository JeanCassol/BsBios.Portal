using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioAgendamentoVisualizacaoController : Controller
    {
        private readonly IConsultaQuotaRelatorio _consultaQuotaRelatorio;

        public RelatorioAgendamentoVisualizacaoController(IConsultaQuotaRelatorio consultaQuotaRelatorio)
        {
            _consultaQuotaRelatorio = consultaQuotaRelatorio;
        }

        public ActionResult GerarRelatorio(Enumeradores.RelatorioDeAgendamento relatorioDeAgendamento, RelatorioAgendamentoFiltroVm filtro)
        {
            ViewBag.Filtro = filtro;
            ViewBag.RelatorioDeAgendamento = relatorioDeAgendamento;
            switch (relatorioDeAgendamento)
            {
                case Enumeradores.RelatorioDeAgendamento.ListagemDeQuotas:
                    ViewBag.TituloDoRelatorio = "Listagem de Quotas";
                    IList<QuotaListagemVm> quotasListadas = _consultaQuotaRelatorio.ListagemDeQuotas(filtro);
                    return View("ListagemDeQuotas", quotasListadas);
                case Enumeradores.RelatorioDeAgendamento.ListagemDeAgendamentos:
                    ViewBag.TituloDoRelatorio = "Listagem de Agendamentos";
                    IList<AgendamentoDeCargaRelatorioListarVm> agendamentosListados = _consultaQuotaRelatorio.ListagemDeAgendamentos(filtro);
                    return View("ListagemDeAgendamentos", agendamentosListados);
                case Enumeradores.RelatorioDeAgendamento.PlanejadoVersusRealizado:
                    ViewBag.TituloDoRelatorio = "Agendamentos Planejado x Realizado";
                    RelatorioDeQuotaPlanejadoVersusRealizadoVm relatorioVm = _consultaQuotaRelatorio.PlanejadoRealizado(filtro);
                    return View("PlanejadoRealizado", relatorioVm);
                case Enumeradores.RelatorioDeAgendamento.PlanejadoVersusRealizadoPorData:
                    ViewBag.TituloDoRelatorio = "Agendamentos Planejado x Realizado por Data";
                    RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm relatorioPorDataVm = _consultaQuotaRelatorio.PlanejadoRealizadoPorData(filtro);
                    return View("PlanejadoRealizadoPorData", relatorioPorDataVm);
                default:
                    var contentResult = new ContentResult {Content = "Opção Inválida"};
                    return contentResult;
            }
        }

    }
}
