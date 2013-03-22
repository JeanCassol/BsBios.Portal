using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class SelecionarFornecedorController : Controller
    {
        public ActionResult Selecionar(FornecedorCadastroVm fornecedorCadastroVm)
        {
            return PartialView("_SelecionarFornecedor",fornecedorCadastroVm);
        }

    }
}
