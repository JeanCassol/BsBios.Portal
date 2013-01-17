using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
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
                Id = 1000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP2000",
                Descricao = "Soja",
                Id = 2000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP3000",
                Descricao = "Milho",
                Id = 3000
            });
        }

        //
        // GET: /Produto/

        public ActionResult Index()
        {

            return View(_produtos);
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            return View("Cadastro");
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProduto)
        {
            var produtoCadastroVm = _produtos.Single(p => p.Id == idProduto);
            return View("Cadastro", produtoCadastroVm);
        }

        [HttpGet]
        public JsonResult Listar()
        {
            return Json(new {registros = _produtos, totalCount = _produtos.Count},JsonRequestBehavior.AllowGet);
        }
    }
}
