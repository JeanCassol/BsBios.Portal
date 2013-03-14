using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoCotacaoFreteController : Controller
    {
        private readonly IList<CotacaoFreteListagemVm> _cotacoesDeFrete ;
        private readonly IList<ProdutoCadastroVm> _produtos;
        private readonly IConsultaUnidadeDeMedida _consultaUnidadeDeMedida ;

        public ProcessoCotacaoFreteController(IConsultaUnidadeDeMedida consultaUnidadeDeMedida)
        {
            _consultaUnidadeDeMedida = consultaUnidadeDeMedida;

            _cotacoesDeFrete = new List<CotacaoFreteListagemVm>();
            _cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
                {
                    Id = 1,
                    CodigoMaterialSap = "SAP1000",
                    Material = "Bio Diesel" ,
                    Quantidade = 10 ,
                    Unidade = "TONELADA",
                    Status =  "ABERTO",
                    DataInicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy") ,
                    DataTermino = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy") 
                });
            _cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
            {
                Id = 1,
                CodigoMaterialSap = "SAP2000",
                Material = "Soja",
                Quantidade = 20,
                Unidade = "TONELADA",
                Status = "PENDENTE",
                DataInicio = DateTime.Today.ToString("dd/MM/yyyy"),
                DataTermino = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy")
            });
            _cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
            {
                Id = 1,
                CodigoMaterialSap = "SAP3000",
                Material = "Trigo",
                Quantidade = 30,
                Unidade = "TONELADA",
                Status = "FECHADO",
                DataInicio = DateTime.Today.AddDays(-3).ToString("dd/MM/yyyy"),
                DataTermino = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy")
            });

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
                Descricao = "Milho",
            });

        }

        //
        // GET: /CotacaoFrete/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Listar()
        {
            return Json(new { registros = _cotacoesDeFrete.OrderByDescending(x => x.DataTermino), totalCount = _cotacoesDeFrete.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            ViewBag.UnidadesDeMedida = _consultaUnidadeDeMedida.ListarTodos();
            
            return View("Cadastro");
        }

        [HttpPost]
        public ActionResult NovoCadastro(CotacaoFreteCadastroVm cotacaoFrete)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacaoFrete)
        {
            return View("Cadastro");
        }

        [HttpPost]
        public ActionResult EditarCadastro()
        {
            return RedirectToAction("Index");
        }

        public ActionResult SelecionarProduto()
        {
            return View("_SelecionarProduto");
        }

    }
}
