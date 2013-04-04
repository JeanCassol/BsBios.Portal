﻿using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;

namespace BsBios.Portal.UI.Controllers
{
    public class ProcessoDeCotacaoDeMaterialAberturaController : Controller
    {
        private readonly IAberturaDeProcessoDeCotacaoService _service;

        public ProcessoDeCotacaoDeMaterialAberturaController(IAberturaDeProcessoDeCotacaoServiceFactory serviceFactory)
        {
            _service = serviceFactory.Construir();
        }

        [HttpPost]
        public JsonResult AbrirProcesso(int idProcessoCotacao)
        {
            try
            {
                _service.Executar(idProcessoCotacao);
                return Json(new { Sucesso = true, Mensagem = "O Processo de Cotação foi aberto com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao abrir o Processo de Cotação. Detalhes: " + ex.Message });
            }
        }
    }
    
}