using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoFornecedorController : Controller
    {
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacao;
        private readonly IConsultaProduto _consultaProduto;
        private readonly IConsultaFornecedor _consultaFornecedor;

        public ProcessoDeCotacaoFornecedorController(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacao, 
            IConsultaProduto consultaProduto, IConsultaFornecedor consultaFornecedor)
        {
            _consultaProcessoDeCotacao = consultaProcessoDeCotacao;
            _consultaProduto = consultaProduto;
            _consultaFornecedor = consultaFornecedor;
        }

        public JsonResult FornecedoresDosProdutos(PaginacaoVm paginacaoVm, int idProcessoCotacao)
        {
            string[] codigoDosProdutos = _consultaProcessoDeCotacao.CodigoDosProdutos(idProcessoCotacao);
            KendoGridVm kendoGridVm = _consultaProduto.FornecedoresDosProdutos(paginacaoVm, codigoDosProdutos);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        public JsonResult FornecedoresGerais(PaginacaoVm paginacaoVm, int idProcessoCotacao, FornecedorDoProdutoFiltro filtro)
        {
            string[] codigoDosProdutos = _consultaProcessoDeCotacao.CodigoDosProdutos(idProcessoCotacao);
            filtro.CodigoDosProdutos = codigoDosProdutos;
            KendoGridVm kendoGridVm = _consultaFornecedor.FornecedoresNaoVinculadosAoProduto(paginacaoVm, filtro);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

    }
}
