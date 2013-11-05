using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class MunicipioController : Controller
    {
        private readonly IConsultaMunicipio _consultaMunicipio;

        public MunicipioController(IConsultaMunicipio consultaMunicipio)
        {
            _consultaMunicipio = consultaMunicipio;
        }

        [HttpGet]
        public JsonResult Buscar(string term)
        {
            return Json(_consultaMunicipio.NomeComecandoCom(term),JsonRequestBehavior.AllowGet);
        }

    }
}
