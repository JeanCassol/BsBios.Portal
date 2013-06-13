using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class EficienciaDeNegociacaoCalculoController : Controller
    {
        private readonly IServicoDeEficienciaDeNegociacao _servicoDeEficienciaDeNegociacao;

        public EficienciaDeNegociacaoCalculoController(IServicoDeEficienciaDeNegociacao servicoDeEficienciaDeNegociacao, IConsultaEficienciaDeNegociacao consultaEficienciaDeNegociacao)
        {
            _servicoDeEficienciaDeNegociacao = servicoDeEficienciaDeNegociacao;
        }

        public JsonResult ListarColunas(int idProcessoCotacao)
        {
            return Json(new { Colunas = _servicoDeEficienciaDeNegociacao.ListarFornecedores(idProcessoCotacao) }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CalcularDetalhe(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            var listaDinamica = _servicoDeEficienciaDeNegociacao.CalcularEficienciaDoItemDoProcesso(idProcessoCotacao, idProcessoCotacaoItem);
            var kendoGridVm = new {Registros = listaDinamica, QuantidadeDeRegistros = listaDinamica.Count};
            var jsonSerializado = JsonConvert.SerializeObject(kendoGridVm, new KeyValuePairConverter());
            return Content(jsonSerializado);
        }

        public ActionResult CalcularResumo(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro)
        {
            //diminui um no tamanho da página porque sem vai ter um registro com o total
            paginacaoVm.PageSize--;
            paginacaoVm.Take--;

            var kendoGridVm = _servicoDeEficienciaDeNegociacao.CalcularResumo(paginacaoVm, filtro);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
