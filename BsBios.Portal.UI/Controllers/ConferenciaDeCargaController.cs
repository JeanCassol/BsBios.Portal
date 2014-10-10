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
        private readonly IConsultaFluxoDeCarga _consultaFluxoDeCarga;

        public ConferenciaDeCargaController(IConsultaRealizacaoDeAgendamento consultaRealizacaoDeAgendamento, IConsultaTerminal consultaTerminal, IConsultaFluxoDeCarga consultaFluxoDeCarga)
        {
            _consultaRealizacaoDeAgendamento = consultaRealizacaoDeAgendamento;
            _consultaTerminal = consultaTerminal;
            _consultaFluxoDeCarga = consultaFluxoDeCarga;
        }

        public ActionResult Pesquisar()
        {
            ViewBag.RealizacoesDeAgendamento = _consultaRealizacaoDeAgendamento.Listar();
            ViewBag.Terminais = _consultaTerminal.ListarTodos();
            ViewBag.FluxosDeCarga = _consultaFluxoDeCarga.Listar();

            return View();
        }

    }
}
