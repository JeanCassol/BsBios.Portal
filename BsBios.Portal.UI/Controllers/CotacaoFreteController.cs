using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class CotacaoFreteController : Controller
    {
        private readonly IList<CotacaoFreteListagemVm> _cotacoesDeFrete ;

        public CotacaoFreteController()
        {
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
            return View("Cadastro");
        }

        [HttpPost]
        public ActionResult NovoCadastro(CotacaoFreteCadastroVm cotacaoFrete)
        {
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idCotacaoFrete)
        {
            return View("Cadastro");
        }

        [HttpPost]
        public ActionResult EditarCadastro()
        {
            return RedirectToAction("Index");
        }


    }
}
