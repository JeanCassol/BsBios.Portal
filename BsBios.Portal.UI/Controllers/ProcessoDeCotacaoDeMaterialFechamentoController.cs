﻿using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoDeMaterialFechamentoController : Controller
    {
        private readonly IFechamentoDeProcessoDeCotacaoService _service;

        public ProcessoDeCotacaoDeMaterialFechamentoController(IFechamentoDeProcessoDeCotacaoServiceFactory serviceFactory)
        {
            _service = serviceFactory.Construir();
        }

        [HttpPost]
        public JsonResult FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _service.Executar(idProcessoCotacao);
                return Json(new { Sucesso = true, Mensagem = "O Processo de Cotação foi fechado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: " + ex.Message });
            }
        }
    }
    
}
