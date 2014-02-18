using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class MonitorDeOrdemDeTransporteController : Controller
    {
        private readonly IConsultaMonitorDeOrdemDeTransporte _consultaMonitorDeOrdemDeTransporte;
        private readonly IConsultaTerminal _consultaTerminal;

        public MonitorDeOrdemDeTransporteController(IConsultaMonitorDeOrdemDeTransporte consultaMonitorDeOrdemDeTransporte, IConsultaTerminal consultaTerminal)
        {
            _consultaMonitorDeOrdemDeTransporte = consultaMonitorDeOrdemDeTransporte;
            _consultaTerminal = consultaTerminal;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Terminais = _consultaTerminal.ListarTodos();
            return View(new MonitorOrdemDeTransporteParametroVm
            {
                InterValoDeAtualizacao = 60,
                NumeroDeRegistrosPorPagina = 10
            });
        }

        [HttpGet]
        public ActionResult Listar(MonitorDeOrdemDeTransporteConfiguracaoVm filtro)
        {
            try
            {
                var materiais = _consultaMonitorDeOrdemDeTransporte.ListarPorMaterial(filtro);

                return Json(new {Sucesso = true, Materiais = materiais}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new {Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex)},JsonRequestBehavior.AllowGet);
            }
        }


    }
}