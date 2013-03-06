﻿using System.Web.Mvc;
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

        public JsonResult FornecedoresGerais(string codigoProduto)
        {
            KendoGridVm kendoGridVm = _consultaFornecedor.FornecedoresNaoVinculadosAoProduto(codigoProduto);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
