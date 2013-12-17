using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{

    [SecurityFilter]
    public class RelatorioDeOrdemDeTransporteController : Controller
    {
        private readonly IConsultaStatusDeOrdemDeTransporte _consulta;

        public RelatorioDeOrdemDeTransporteController(IConsultaStatusDeOrdemDeTransporte consulta)
        {
            _consulta = consulta;
        }

        public ActionResult Relatorio()
        {
            ViewBag.StatusDeOrdemDeTransporte = _consulta.Listar();

            return View();
        }
    }

}
