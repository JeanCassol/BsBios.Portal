﻿using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoDeMaterialFechamentoController : Controller
    {
        private readonly IFechamentoDeProcessoDeCotacaoDeMaterialService _service;

        public ProcessoDeCotacaoDeMaterialFechamentoController(IFechamentoDeProcessoDeCotacaoDeMaterialService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResult FecharProcesso(ProcessoDeCotacaoDeMaterialFechamentoInfoVm processoDeCotacaoFechamentoVm)
        {
            try
            {
                _service.Executar(processoDeCotacaoFechamentoVm);
                return Json(new { Sucesso = true, Mensagem = "O Processo de Cotação foi fechado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: " + ExceptionUtil.ExibeDetalhes(ex) });
            }
        }
    }
    
}
