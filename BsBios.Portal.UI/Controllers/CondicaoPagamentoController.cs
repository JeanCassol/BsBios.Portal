using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CondicaoPagamentoController : Controller
    {
        private readonly IConsultaCondicaoPagamento _consultaCondicaoPagamento;
        public CondicaoPagamentoController(IConsultaCondicaoPagamento consultaCondicaoPagamento)
        {
            _consultaCondicaoPagamento = consultaCondicaoPagamento;
        }

        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacao, CondicaoDePagamentoCadastroVm filtro)
        {
            IList<CondicaoDePagamentoCadastroVm> registros = _consultaCondicaoPagamento.Listar(paginacao, filtro);
            return Json(new { registros, totalCount = 10 }, JsonRequestBehavior.AllowGet);
        }
    }
}
