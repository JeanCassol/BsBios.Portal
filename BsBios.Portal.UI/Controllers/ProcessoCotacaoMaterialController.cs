using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoCotacaoMaterialController : Controller
    {
        //private readonly IList<CotacaoFreteListagemVm> _cotacoesDeFrete ;
        //private readonly IList<ProdutoCadastroVm> _produtos;
        //private readonly IList<ItinerarioCadastroVm> _itinerarios ;
        //private readonly IList<CentroCadastroVm> _centros;
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacaoDeMaterial;

        public ProcessoCotacaoMaterialController(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial)
        {
            _consultaProcessoDeCotacaoDeMaterial = consultaProcessoDeCotacaoDeMaterial;
            //_cotacoesDeFrete = new List<CotacaoFreteListagemVm>();
            //_cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
            //    {
            //        Id = 1,
            //        CodigoMaterialSap = "SAP1000",
            //        Material = "Bio Diesel" ,
            //        Quantidade = 10 ,
            //        Unidade = "TONELADA",
            //        Status =  "ABERTO",
            //        DataInicio = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy") ,
            //        DataTermino = DateTime.Today.AddDays(1).ToString("dd/MM/yyyy") 
            //    });
            //_cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
            //{
            //    Id = 1,
            //    CodigoMaterialSap = "SAP2000",
            //    Material = "Soja",
            //    Quantidade = 20,
            //    Unidade = "TONELADA",
            //    Status = "PENDENTE",
            //    DataInicio = DateTime.Today.ToString("dd/MM/yyyy"),
            //    DataTermino = DateTime.Today.AddDays(2).ToString("dd/MM/yyyy")
            //});
            //_cotacoesDeFrete.Add(new CotacaoFreteListagemVm()
            //{
            //    Id = 1,
            //    CodigoMaterialSap = "SAP3000",
            //    Material = "Trigo",
            //    Quantidade = 30,
            //    Unidade = "TONELADA",
            //    Status = "FECHADO",
            //    DataInicio = DateTime.Today.AddDays(-3).ToString("dd/MM/yyyy"),
            //    DataTermino = DateTime.Today.AddDays(-1).ToString("dd/MM/yyyy")
            //});

            //_produtos = new List<ProdutoCadastroVm>();
            //_produtos.Add(new ProdutoCadastroVm()
            //{
            //    CodigoSap = "SAP1000",
            //    Descricao = "Bio Diesel",
            //});
            //_produtos.Add(new ProdutoCadastroVm()
            //{
            //    CodigoSap = "SAP2000",
            //    Descricao = "Soja",
            //});
            //_produtos.Add(new ProdutoCadastroVm()
            //{
            //    CodigoSap = "SAP3000",
            //    Descricao = "Milho",
            //});

            //_itinerarios = new List<ItinerarioCadastroVm>();
            //_itinerarios.Add(new ItinerarioCadastroVm(){Id = 1, Descricao = "Itinerário 1"});
            //_itinerarios.Add(new ItinerarioCadastroVm() { Id = 2, Descricao = "Itinerário 2" });

            //_centros = new List<CentroCadastroVm>();
            //_centros.Add(new CentroCadastroVm(){Id = 1, Descricao = "Centro 1"});
            //_centros.Add(new CentroCadastroVm() { Id = 2, Descricao = "Centro 2" });
        }

        //
        // GET: /CotacaoFrete/

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm)
        {
            var kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.Listar(paginacaoVm, null);
            return Json(new { registros = kendoGridVm.Registros, totalCount = kendoGridVm.QuantidadeDeRegistros }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            //ViewBag.Materiais = _produtos;
            //ViewBag.Itinerarios = _itinerarios;
            //ViewBag.Centros = _centros;
            
            return View("Cadastro", new ProcessoCotacaoMaterialCadastroVm 
            {
                DescricaoStatus = " Nâo Iniciado"/*,
                Quantidade = 10,
                DataInicioLeilao = DateTime.Today.ToShortDateString()*/});
        }

        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            ProcessoCotacaoMaterialCadastroVm cadastro = _consultaProcessoDeCotacaoDeMaterial.ConsultaProcesso(idProcessoCotacao);
            return View("Cadastro", cadastro);
        }

        [HttpGet]
        public JsonResult ListarFornecedores(int idProcessoCotacao)
        {
            var kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.FornecedoresParticipantes(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
            
        }

        public ActionResult SelecionarFornecedores(int idProcessoCotacao, string codigoProduto)
        {
            ViewData["CodigoProduto"] = codigoProduto;
            ViewData["IdProcessoCotacao"] = idProcessoCotacao; 
            return PartialView("_SelecionarFornecedor");
        }

    }
}
