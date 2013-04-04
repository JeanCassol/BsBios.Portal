using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
        private readonly IList<ProdutoCadastroVm> _produtos; 
        public ProdutoController(IConsultaProduto consultaProduto)
        {
            _consultaProduto = consultaProduto;
            _produtos = new List<ProdutoCadastroVm>();
            _produtos.Add(new ProdutoCadastroVm()
            {
                Codigo = "SAP1000",
                Descricao = "Bio Diesel",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                Codigo = "SAP2000",
                Descricao = "Soja",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                Codigo = "SAP3000",
                Descricao = "Milhos",
            });
            _produtos.Add(new ProdutoCadastroVm()
                {
                    Codigo =  "SAP4000",
                    Descricao = "Farelo de Soja" ,
                });
            _produtos.Add(new ProdutoCadastroVm()
            {
                Codigo = "SAP5000",
                Descricao = "Produto 5",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                Codigo = "SAP6000",
                Descricao = "Produto 6",
            });
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            return View("Cadastro");
        }

        [HttpGet]
        public ViewResult EditarCadastro(string codigoProduto)
        {
            var produtoCadastroVm = _produtos.Single(p => p.Codigo == codigoProduto);
            return View("Cadastro", produtoCadastroVm);
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
