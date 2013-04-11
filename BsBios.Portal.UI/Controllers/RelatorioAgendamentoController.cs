﻿using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class RelatorioAgendamentoController : Controller
    {
        private readonly IConsultaFluxoDeCarga _consultaFluxoDeCarga;

        public RelatorioAgendamentoController(IConsultaFluxoDeCarga consultaFluxoDeCarga)
        {
            _consultaFluxoDeCarga = consultaFluxoDeCarga;
        }

        public ActionResult Relatorio()
        {
            ViewBag.FluxosDeCarga = _consultaFluxoDeCarga.Listar();
            return View();
        }

        public string Gerar()
        {
            return "Abrindo uma janela para exibir dados dos relatórios";
        }
    }
}
