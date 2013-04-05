using System.Web.Mvc;
using BsBios.Portal.Infra.Services.Contracts;

namespace BsBios.Portal.UI.Controllers
{
    public class CriptografiaController : Controller
    {
        private readonly IProvedorDeCriptografia _provedorDeCriptografia ;

        public CriptografiaController(IProvedorDeCriptografia provedorDeCriptografia)
        {
            _provedorDeCriptografia = provedorDeCriptografia;
        }

        [HttpGet]
        public ContentResult Hash(string dado)
        {
            return Content(_provedorDeCriptografia.Criptografar(dado));
        }

    }
}
