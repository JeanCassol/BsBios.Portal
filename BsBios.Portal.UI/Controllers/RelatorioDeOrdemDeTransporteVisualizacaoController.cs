using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeOrdemDeTransporteVisualizacaoController : Controller
    {
        private readonly IConsultaRelatorioDeOrdemDeTransporte _consulta;

        public RelatorioDeOrdemDeTransporteVisualizacaoController(IConsultaRelatorioDeOrdemDeTransporte consulta)
        {
            _consulta = consulta;
        }

        public ActionResult GerarRelatorio(Enumeradores.RelatorioDeOrdemDeTransporte relatorio, RelatorioDeOrdemDeTransporteFiltroVm filtro)
        {
            ViewBag.Filtro = filtro;
            IList<RelatorioDeOrdemDeTransporteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);

            switch (relatorio)
            {
                case Enumeradores.RelatorioDeOrdemDeTransporte.Resumido:
                    ViewBag.TituloDoRelatorio = "Relatório de Ordem de Transporte - Resumido";
                    ViewBag.Resumido = true;
                    break;
                case Enumeradores.RelatorioDeOrdemDeTransporte.Completo:
                    ViewBag.TituloDoRelatorio = "Relatório de Ordem de Transporte - Completo";
                    ViewBag.Resumido = false;
                    break;
                default:
                    return Content("Opção do relatório inválida");

            }

            return View("ListagemAnalitica", processos);

        }
    }
}