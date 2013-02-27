using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
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

        //[HttpGet]
        //public JsonResult Listar(PaginacaoVm paginacaoVm)
        //{
        //    KendoGridVm kendoGridVm = _consultaFornecedor
        //    return Json(new { registros = , totalCount =  }, JsonRequestBehavior.AllowGet);
        //}
        //[HttpGet]
        //public PartialViewResult Index()
        //{
        //    return View();
        //}

        [HttpGet]
        public JsonResult FornecedoresDoProduto(string codigoProduto)
        {
            KendoGridVm kendoGridVm = _consultaFornecedor.FornecedoresDoProduto(codigoProduto);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
