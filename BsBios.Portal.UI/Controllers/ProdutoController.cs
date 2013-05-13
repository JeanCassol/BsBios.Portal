using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProdutoController : Controller
    {
        private readonly IConsultaProduto _consultaProduto;
        public ProdutoController(IConsultaProduto consultaProduto)
        {
            _consultaProduto = consultaProduto;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacao, ProdutoCadastroVm filtro)
        {
            return Json(_consultaProduto.Listar(paginacao, filtro), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult FornecedoresDoProduto(PaginacaoVm paginacaoVm, string codigoProduto)
        {
            KendoGridVm kendoGridVm = _consultaProduto.FornecedoresDoProduto(paginacaoVm, codigoProduto);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        public ViewResult Cadastro(string codigoProduto)
        {
            ProdutoCadastroVm produtoCadastroVm = _consultaProduto.ConsultaPorCodigo(codigoProduto);
            return View(produtoCadastroVm);
        }
    }
}
