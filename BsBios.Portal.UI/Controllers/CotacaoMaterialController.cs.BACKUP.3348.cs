using System;
using System.Collections.Generic;
<<<<<<< HEAD
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.UI.Filters;
=======
using System.Web.Mvc;
>>>>>>> b651f1363b422a33354ebef50642b7e9c93d20a6
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
<<<<<<< HEAD
    [SecurityFilter]
    public class CotacaoMaterialController : Controller
    {
        private readonly IList<CotacaoFreteListagemVm> _cotacoesDeFrete ;
        private readonly IList<ProdutoCadastroVm> _produtos;
        private readonly IList<ItinerarioCadastroVm> _itinerarios ;
        private readonly IList<CentroCadastroVm> _centros;

        public CotacaoMaterialController()
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

            _produtos = new List<ProdutoCadastroVm>();
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "SAP1000",
                Descricao = "Bio Diesel",
=======
    public class CotacaoMaterialController : Controller
    {
        private readonly List<ProdutoCadastroVm> _produtos;

        public CotacaoMaterialController()
        {
            _produtos = new List<ProdutoCadastroVm>();
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "MAT0001",
                Descricao = "MATERIAL 0001",
>>>>>>> b651f1363b422a33354ebef50642b7e9c93d20a6
                Id = 1000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
<<<<<<< HEAD
                CodigoSap = "SAP2000",
                Descricao = "Soja",
=======
                CodigoSap = "MAT0002",
                Descricao = "MATERIAL 0002",
>>>>>>> b651f1363b422a33354ebef50642b7e9c93d20a6
                Id = 2000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
<<<<<<< HEAD
                CodigoSap = "SAP3000",
                Descricao = "Milho",
                Id = 3000
            });

            _itinerarios = new List<ItinerarioCadastroVm>();
            _itinerarios.Add(new ItinerarioCadastroVm(){Id = 1, Descricao = "Itinerário 1"});
            _itinerarios.Add(new ItinerarioCadastroVm() { Id = 2, Descricao = "Itinerário 2" });

            _centros = new List<CentroCadastroVm>();
            _centros.Add(new CentroCadastroVm(){Id = 1, Descricao = "Centro 1"});
            _centros.Add(new CentroCadastroVm() { Id = 2, Descricao = "Centro 2" });
        }

        //
        // GET: /CotacaoFrete/

        [HttpGet]
=======
                CodigoSap = "MAT0003",
                Descricao = "MATERIAL 0003",
                Id = 3000
            });
        }

>>>>>>> b651f1363b422a33354ebef50642b7e9c93d20a6
        public ActionResult Index()
        {
            return View();
        }
<<<<<<< HEAD
        [HttpGet]
        public JsonResult Listar()
        {
            return Json(new { registros = _cotacoesDeFrete.OrderByDescending(x => x.DataTermino), totalCount = _cotacoesDeFrete.Count }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            ViewBag.Materiais = _produtos;
            ViewBag.Itinerarios = _itinerarios;
            ViewBag.Centros = _centros;
            
            return View("Cadastro", new CotacaoFreteCadastroVm 
            {
                DescricaoStatus = "ABERTO",
                QuantidadeMaterial = 10,
                DataInicioLeilao = DateTime.Today.ToShortDateString()});
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
=======
        public JsonResult ListarFornecedores(int idCotacao)
        {

            var registros = new List<FornecedorCotacaoVm>()
                {
                    new FornecedorCotacaoVm()
                        {
                            Codigo = "FORNEC0001",
                            Nome = "FORNECEDOR 0001",
                            Cnpj = "66.637.135/0001-70",
                            Selecionado = false
                        },
                    new FornecedorCotacaoVm()
                        {
                            Codigo = "FORNEC0002",
                            Nome = "FORNECEDOR 0002",
                            Cnpj = "46.617.806/0001-24",
                            Selecionado = true
                        },
                    new FornecedorCotacaoVm()
                        {
                            Codigo = "FORNEC0003",
                            Nome = "FORNECEDOR 0003",
                            Cnpj = "58.444.546/0001-11",
                            Selecionado = false
                        }
                };

            return Json(new {registros}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Listar(PaginacaoVm paginacao)
        {
            var registros = new List<CotacaoMaterialListagemVm>()
                {
                    new CotacaoMaterialListagemVm()
                        {
                            CodigoMaterialSap = "MAT0001",
                            DataInicio = DateTime.Today.AddDays(-4).ToShortDateString(),
                            DataTermino = DateTime.Today.AddDays(-2).ToShortDateString(),
                            Id = 1,
                            Material = "MATERIAL 0001",
                            Quantidade = 100,
                            Status = "FECHADO",
                            Unidade = "PEÇA"
                        },
                    new CotacaoMaterialListagemVm()
                        {
                            CodigoMaterialSap = "MAT0002",
                            DataInicio = DateTime.Today.AddDays(-2).ToShortDateString(),
                            DataTermino = DateTime.Today.ToShortDateString(),
                            Id = 2,
                            Material = "MATERIAL 0002",
                            Quantidade = 200,
                            Status = "ABERTO",
                            Unidade = "PEÇA"
                        },
                    new CotacaoMaterialListagemVm()
                        {
                            CodigoMaterialSap = "MAT0003",
                            DataInicio = DateTime.Today.ToShortDateString(),
                            DataTermino = DateTime.Today.AddDays(2).ToShortDateString(),
                            Id = 1,
                            Material = "MATERIAL 0003",
                            Quantidade = 300,
                            Status = "PENDENTE",
                            Unidade = "PEÇA"
                        }
                };

            return Json(new {registros, totalCount = 1}, JsonRequestBehavior.AllowGet);
        }    

        public ActionResult Editar()
        {
            ViewBag.Materiais = _produtos;
            var cotacaoVm = new CotacaoMaterialCadastroVm()
                {
                    DataInicioLeilao = DateTime.Today.ToShortDateString(),
                    DataTerminoLeilao = DateTime.Today.AddDays(2).ToShortDateString(),
                    DataValidadeCotacaoInicial = DateTime.Today.AddDays(3).ToShortDateString(),
                    DataValidadeCotacaoFinal = DateTime.Today.AddDays(30).ToShortDateString(),
                    DescricaoMaterial = "MATERIAL 0001 ",
                    DescricaoStatus = "ABERTO",
                    Id = 1,
                    IdMaterial = 2000,
                    Observacoes = "Observações do Leilão",
                    QuantidadeMaterial = 100,
                    Requisitos = "Requisitos do Leilão"

                };
            return View("Cadastro", cotacaoVm);
        }
    }
}
>>>>>>> b651f1363b422a33354ebef50642b7e9c93d20a6
