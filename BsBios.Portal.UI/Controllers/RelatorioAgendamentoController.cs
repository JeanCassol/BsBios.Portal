using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioAgendamentoController : Controller
    {
        private readonly IConsultaFluxoDeCarga _consultaFluxoDeCarga;
        private readonly IConsultaTerminal _consultaTerminal;

        public RelatorioAgendamentoController(IConsultaFluxoDeCarga consultaFluxoDeCarga, IConsultaTerminal consultaTerminal)
        {
            _consultaFluxoDeCarga = consultaFluxoDeCarga;
            _consultaTerminal = consultaTerminal;
        }

        public ActionResult Relatorio()
        {
            ViewBag.FluxosDeCarga = _consultaFluxoDeCarga.Listar();
            ViewBag.Terminais = _consultaTerminal.ListarTodos();
            return View();
        }
    }
}
