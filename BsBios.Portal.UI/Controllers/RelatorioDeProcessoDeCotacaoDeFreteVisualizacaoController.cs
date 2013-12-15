using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using Microsoft.Ajax.Utilities;

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
            ViewBag.Filtro = filtro;

            switch (relatorio)
            {

                case Enumeradores.RelatorioDeProcessosDeCotacaoDeFrete.Analitico:
                    IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
                    ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";
                    return View("ListagemAnalitica", processos);

                case Enumeradores.RelatorioDeProcessosDeCotacaoDeFrete.SinteticoComSoma:
                    IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> registrosComSoma = _consulta.ListagemSinteticaComSoma(filtro);
                    ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Sintético com Soma";
                    return View("ListagemSintetica", registrosComSoma);

                case Enumeradores.RelatorioDeProcessosDeCotacaoDeFrete.SinteticoComMedia:
                    IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> registrosComMedia = _consulta.ListagemSinteticaComMedia(filtro);
                    ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Sintético com Média";
                    return View("ListagemSintetica", registrosComMedia);

                default:
                    return Content("Opção do relatório inválida");
            }

            
        }

    }
}
