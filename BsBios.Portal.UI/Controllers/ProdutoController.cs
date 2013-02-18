using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProdutoController : Controller
    {
        private readonly IList<ProdutoCadastroVm> _produtos; 
        public ProdutoController()
        {
            _produtos = new List<ProdutoCadastroVm>();
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP1000",
                Descricao = "Bio Diesel",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP2000",
                Descricao = "Soja",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP3000",
                Descricao = "Milhos",
            });
            _produtos.Add(new ProdutoCadastroVm()
                {
                    CodigoSap =  "SAP4000",
                    Descricao = "Farelo de Soja" ,
                });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP5000",
                Descricao = "Produto 5",
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP6000",
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
            var produtoCadastroVm = _produtos.Single(p => p.CodigoSap == codigoProduto);
            return View("Cadastro", produtoCadastroVm);
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacao, ProdutoCadastroVm filtro)
        {
            IQueryable<ProdutoCadastroVm> retorno =  _produtos.AsQueryable();
            if (!string.IsNullOrEmpty(filtro.CodigoSap))
            {
                retorno = retorno.Where(p => p.CodigoSap == filtro.CodigoSap);
            }
            if (!string.IsNullOrEmpty(filtro.Descricao))
            {
                retorno = retorno.Where(p => p.Descricao.ToLower().Contains(filtro.Descricao.ToLower()));
            }
            int skip = (paginacao.Page - 1) * paginacao.PageSize;
            return Json(new {registros = retorno.Skip(skip).Take(paginacao.Take).ToList(), totalCount = retorno.Count()},JsonRequestBehavior.AllowGet);
        }
    }
}
