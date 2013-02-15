using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
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
                Id = 1000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "MAT0002",
                Descricao = "MATERIAL 0002",
                Id = 2000
            });
            _produtos.Add(new ProdutoCadastroVm()
            {
                CodigoSap = "MAT0003",
                Descricao = "MATERIAL 0003",
                Id = 3000
            });
        }

        public ActionResult Index()
        {
            return View();
        }
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