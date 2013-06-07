using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class EficienciaDeNegociacaoController : Controller
    {
        private readonly IServicoDeEficienciaDeNegociacao _servicoDeEficienciaDeNegociacao;

        public EficienciaDeNegociacaoController(IServicoDeEficienciaDeNegociacao servicoDeEficienciaDeNegociacao)
        {
            _servicoDeEficienciaDeNegociacao = servicoDeEficienciaDeNegociacao;
        }

        public JsonResult ListarColunas(string numeroDaRequisicao, string numeroDoItem)
        {
            return Json(new {Colunas = _servicoDeEficienciaDeNegociacao.ListarFornecedores(numeroDaRequisicao, numeroDoItem) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularDetalhe(string numeroDaRequisicao, string numeroDoItem)
        {
            //return Json(new {Registros = _servicoDeEficienciaDeNegociacao.CalcularEficienciaDoItemDoProcesso("", "")},JsonRequestBehavior.AllowGet);
            var listaDinamica = _servicoDeEficienciaDeNegociacao.CalcularEficienciaDoItemDoProcesso(numeroDaRequisicao, numeroDoItem);
            var kendoGridVm = new {Registros = listaDinamica, QuantidadeDeRegistros = listaDinamica.Count};
            //var jsonSerializado = JsonConvert.SerializeObject(listaDinamica, new KeyValuePairConverter());
            var jsonSerializado = JsonConvert.SerializeObject(kendoGridVm, new KeyValuePairConverter());
            return Content(jsonSerializado);
        }

        public ActionResult CalcularResumo(string numeroDaRequisicao, string numeroDoItem)
        {
            var kendoGridVm = new
                {
                    QuantidadeDeRegistros = 2,
                    Registros = new List<dynamic>
                        {
                            new
                                {
                                    NumeroDaRequisicao = "REQ001",
                                    NumeroDoItem = "ITEM001",
                                    Produto = "Rolamento RT254",
                                    PercentualDeEficiencia = 14.28,
                                    ValorDaEficiencia = 30000
                                },
                            new
                                {
                                    NumeroDaRequisicao = "REQ001",
                                    NumeroDoItem = "ITEM002",
                                    Produto = "Conserto Ar Condicionado",
                                    PercentualDeEficiencia = 5.55,
                                    ValorDaEficiencia = 1000
                                },
                            new
                                {
                                    Produto = "TOTAL",
                                    PercentualDeEficiencia = 5.77,
                                    ValorDaEficiencia = 31000
                                }


                        }
                };
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
