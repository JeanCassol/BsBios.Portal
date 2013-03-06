using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoStatusServiceController : Controller
    {
        private readonly IProcessoDeCotacaoStatusService _processoDeCotacaoStatusService;

        public ProcessoDeCotacaoStatusServiceController(IProcessoDeCotacaoStatusService processoDeCotacaoStatusService)
        {
            _processoDeCotacaoStatusService = processoDeCotacaoStatusService;
        }

        [HttpPost]
        public JsonResult AbrirProcesso(int idProcessoCotacao)
        {
            try
            {
                _processoDeCotacaoStatusService.AbrirProcesso(idProcessoCotacao);
                return Json(new {Sucesso = true, Mensagem = "O Processo de Cotação foi aberto com sucesso."});

            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = "Ocorreu um erro ao abrir o Processo de Cotação. Detalhes: " + ex.Message});
            }
        }

        [HttpPost]
        public JsonResult FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _processoDeCotacaoStatusService.FecharProcesso(idProcessoCotacao);
                return Json(new { Sucesso = true, Mensagem = "O Processo de Cotação foi fechado com sucesso." });

            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: " + ex.Message });
            }
        }
        
    }
}
