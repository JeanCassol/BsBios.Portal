using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.UI.Controllers.CustomActionResult;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class RelatorioDeProcessoDeCotacaoDeFreteVisualizacaoController : BaseController
    {

        private readonly IConsultaRelatorioDeProcessoDeCotacaoDeFrete _consulta;

        public RelatorioDeProcessoDeCotacaoDeFreteVisualizacaoController(IConsultaRelatorioDeProcessoDeCotacaoDeFrete consulta)
        {
            _consulta = consulta;
        }

        [SecurityFilter]
        [HttpGet]
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

        [SecurityFilter]
        [HttpGet]
        public ActionResult GerarRelatorioEmExcel(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            ViewBag.Filtro = filtro;
            ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";

            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
            string html = RenderPartialViewToString("ListagemAnalitica", processos);

            var downloadFileActionResult = new DownloadFileActionResult(html, "RelatorioDeProcessoDeCotacaoAnalitico.xls");

            return downloadFileActionResult;
        }

        [HttpGet]
        public ActionResult GerarParaDownload()
        {
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Status = (int)Enumeradores.StatusProcessoCotacao.Aberto
            };
            ViewBag.Filtro = filtro;
            ViewBag.TituloDoRelatorio = "Relatório de Processo de Cotação de Frete - Analítico";


            string html = GerarHtml(filtro);

            string linkDoRelatorioAberto = this.GerarEndereco(PersistirArquivo(html, "RelatorioDeProcessoDeCotacaoDeFreteAberto"));

            filtro.Status = (int)Enumeradores.StatusProcessoCotacao.Fechado;
            filtro.DataDeFechamento = DateTime.Today.AddDays(-1).ToShortDateString();

            html = GerarHtml(filtro);

            string linkDoRelatorioFechado = this.GerarEndereco(PersistirArquivo(html,"RelatorioDeProcessoDeCotacaoDeFreteFechado"));


            return Json(new { LinkDoRelatorioAberto = linkDoRelatorioAberto, LinkDoRelatorioFechado = linkDoRelatorioFechado }, JsonRequestBehavior.AllowGet);
        }

        private string GerarHtml(RelatorioDeProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> processos = _consulta.ListagemAnalitica(filtro);
            return RenderPartialViewToString("ListagemAnalitica", processos);

        }

        private string PersistirArquivo(string html, string nomeDoRelatorio)
        {

            byte[] byteArray = Encoding.ASCII.GetBytes(html);

            string nomeDoArquivo = String.Format("{0}{1}.xls",nomeDoRelatorio, DateTime.Now.ToString("yyyyMMddhhmmss"));

            var fileStream = new FileStream(string.Format("{0}\\Download\\Relatorio\\{1}", 
                HttpContext.Request.MapPath(HttpContext.Request.ApplicationPath), nomeDoArquivo), FileMode.Create);

            fileStream.Write(byteArray, 0, byteArray.Length);

            fileStream.Close();

            return nomeDoArquivo;
        }

        private string GerarEndereco(string nomeDoArquivo)
        {
            string endereco = string.Format("{0}://{1}{2}Download/Relatorio/{3}", Request.Url.Scheme, Request.Url.Authority, Url.Content("~"), nomeDoArquivo);
            return endereco;
        }


    }
}
