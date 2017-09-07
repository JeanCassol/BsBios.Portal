using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class SelecionarCompradorController : Controller
    {
        public ActionResult Selecionar(UsuarioCadastroVm comprador)
        {
            return PartialView("_SelecionarComprador", comprador);
        }
    }
}