using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{

    [SecurityFilter]
    public class RelatorioDeOrdemDeTransporteController : Controller
    {
        private readonly IConsultaStatusDeOrdemDeTransporte _consultaStatusDeOrdemDeTransporte;
        private readonly IConsultaTerminal _consultaTerminal;

        public RelatorioDeOrdemDeTransporteController(IConsultaStatusDeOrdemDeTransporte consultaStatusDeOrdemDeTransporte, IConsultaTerminal consultaTerminal)
        {
            _consultaStatusDeOrdemDeTransporte = consultaStatusDeOrdemDeTransporte;
            _consultaTerminal = consultaTerminal;
        }

        public ActionResult Relatorio()
        {
            ViewBag.StatusDeOrdemDeTransporte = _consultaStatusDeOrdemDeTransporte.Listar();
            ViewBag.Terminais = _consultaTerminal.ListarTodos();

            return View();
        }
    }

}
