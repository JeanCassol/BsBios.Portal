using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioDeProcessoDeCotacaoDeFreteVisualizacaoController : Controller
    {

        private readonly IConsultaRelatorioDeProcessoDeCotacaoDeFrete _consulta;

        public RelatorioDeProcessoDeCotacaoDeFreteVisualizacaoController(IConsultaRelatorioDeProcessoDeCotacaoDeFrete consulta)
        {
            _consulta = consulta;
        }

        public ActionResult GerarRelatorio(Enumeradores.RelatorioDeProcessosDeCotacaoDeFrete relatorio, RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
            ViewBag.Filtro = filtro;
            ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";
            return View("ListagemAnalitica", processos);
        }

    }
}
