using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ItinerarioController : Controller
    {
        private readonly IConsultaItinerario _consultaItinerario;

        public ItinerarioController(IConsultaItinerario consultaItinerario)
        {
            _consultaItinerario = consultaItinerario;
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, ItinerarioFiltroVm filtro)
        {
            return Json(_consultaItinerario.Listar(paginacaoVm, filtro), JsonRequestBehavior.AllowGet);
        }
    }
}