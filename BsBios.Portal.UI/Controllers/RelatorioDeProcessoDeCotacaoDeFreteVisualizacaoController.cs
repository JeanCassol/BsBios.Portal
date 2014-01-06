using System.Collections.Generic;
using System.IO;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Controllers.CustomActionResult;
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

        //public ActionResult GerarRelatorioEmExcel(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        //{

        //    ViewBag.Filtro = filtro;

        //    IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
        //    ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";
        //    View("ListagemAnalitica", processos).ExecuteResult(ControllerContext);
        //    Response.ContentType = "application/vnd.ms-excel";
        //    Response.ContentEncoding = System.Text.Encoding.UTF8;
        //    Response.HeaderEncoding = System.Text.Encoding.UTF8;
        //    Response.AddHeader("content-disposition", "attachment; filename=Relatorio.xls");

        //    //Stream outputStream = this.Response.OutputStream;

        //    //long tamanhoDoConteudo = outputStream.Length;

        //    //var dadosParaSalvar = new byte[tamanhoDoConteudo];

        //    //outputStream.Read(dadosParaSalvar, 0,(int) tamanhoDoConteudo);

        //    //var fileStream = new FileStream(".\\RelatorioSalvo.xls",FileMode.Create);

        //    //fileStream.Write(dadosParaSalvar, 0, (int) tamanhoDoConteudo);

        //    //fileStream.Close();

        //    return null;
        //}

        public ActionResult GerarRelatorioEmExcel(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            ViewBag.Filtro = filtro;
            ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";

            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
            string html = RenderPartialViewToString("ListagemAnalitica", processos);

            html = html.Replace("\n", "");
            html = html.Replace("\r", "");
            html = html.Replace("  ", "");

            var downloadFileActionResult = new DownloadFileActionResult(html, "RelatorioDeProcessoDeCotacaoAnalitico.xls");

            return downloadFileActionResult;
        }

        protected string RenderPartialViewToString(string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.RouteData.GetRequiredString("action");

            ViewData.Model = model;

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
                ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                return sw.GetStringBuilder().ToString();

            }
        }

    }
}
