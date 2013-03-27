using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ConferenciaDeCargaController : Controller
    {
        private readonly IConsultaRealizacaoDeAgendamento _consultaRealizacaoDeAgendamento;

        public ConferenciaDeCargaController(IConsultaRealizacaoDeAgendamento consultaRealizacaoDeAgendamento)
        {
            _consultaRealizacaoDeAgendamento = consultaRealizacaoDeAgendamento;
        }

        public ActionResult Pesquisar()
        {
            ViewBag.RealizacoesDeAgendamento = _consultaRealizacaoDeAgendamento.Listar();
            return View();
        }

    }
}
