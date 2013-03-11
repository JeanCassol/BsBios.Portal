using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class FornecedorController : Controller
    {
        private readonly IConsultaFornecedor _consultaFornecedor;
        public FornecedorController(IConsultaFornecedor consultaFornecedor)
        {
            _consultaFornecedor = consultaFornecedor;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult FornecedoresGerais(PaginacaoVm paginacaoVm, string codigoProduto)
        {
            KendoGridVm kendoGridVm = _consultaFornecedor.FornecedoresNaoVinculadosAoProduto(paginacaoVm, codigoProduto);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, string nome)
        {
            KendoGridVm kendoGridVm = _consultaFornecedor.Listar(paginacaoVm, nome);
            return Json(kendoGridVm,JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ViewResult Cadastro(string codigoFornecedor)
        {
            FornecedorCadastroVm vieModel = _consultaFornecedor.ConsultaPorCodigo(codigoFornecedor);
            return View(vieModel);
        }

        [HttpGet]
        public JsonResult ProdutosDoFornecedor(string codigoFornecedor)
        {
            KendoGridVm kendoGridVm = _consultaFornecedor.ProdutosDoFornecedor(codigoFornecedor);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
