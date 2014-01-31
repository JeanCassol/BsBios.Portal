using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ConferenciaDeCargaController : Controller
    {
        private readonly IConsultaRealizacaoDeAgendamento _consultaRealizacaoDeAgendamento;
        private readonly IConsultaTerminal _consultaTerminal;

        public ConferenciaDeCargaController(IConsultaRealizacaoDeAgendamento consultaRealizacaoDeAgendamento, IConsultaTerminal consultaTerminal)
        {
            _consultaRealizacaoDeAgendamento = consultaRealizacaoDeAgendamento;
            _consultaTerminal = consultaTerminal;
        }

        public ActionResult Pesquisar()
        {
            ViewBag.RealizacoesDeAgendamento = _consultaRealizacaoDeAgendamento.Listar();
            ViewBag.Terminais = _consultaTerminal.ListarTodos();
            return View();
        }

    }
}
