﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class MonitorDeOrdemDeTransporteController : Controller
    {
        private readonly IConsultaMonitorDeOrdemDeTransporte _consulta;

        public MonitorDeOrdemDeTransporteController(IConsultaMonitorDeOrdemDeTransporte consulta)
        {
            _consulta = consulta;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(new MonitorOrdemDeTransporteParametroVm
            {
                DataInicial = DateTime.Today.AddMonths(-1).ToShortDateString(),
                DataFinal = DateTime.Today.ToShortDateString(),
                InterValoDeAtualizacao = 15
            });
        }

        [HttpGet]
        public ActionResult Listar(MonitorDeOrdemDeTransporteFiltroVm filtro)
        {
            try
            {
            IList<MonitorDeOrdemDeTransporteVm> materiais = _consulta.Listar(filtro);

                return Json(new {Sucesso = true, Materiais = materiais}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                return Json(new {Sucesso = false, Mensagem = ExceptionUtil.ExibeDetalhes(ex)},JsonRequestBehavior.AllowGet);
            }
        }


    }
}