using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoDeFreteFechamentoController : Controller
    {
        private readonly IFechamentoDeProcessoDeCotacaoDeFreteService _service;

        public ProcessoDeCotacaoDeFreteFechamentoController(IFechamentoDeProcessoDeCotacaoServiceFactory serviceFactory)
        {
            _service = serviceFactory.Construir();
        }

        [HttpPost]
        public JsonResult FecharProcesso(int idProcessoCotacao)
        {
            try
            {
                _service.Executar(idProcessoCotacao);
                return Json(new {Sucesso = true, Mensagem = "O Processo de Cotação foi fechado com sucesso."});
            }
            catch (ComunicacaoSapException ex)
            {
                return Json(new { Sucesso = false, ex.MediaType, Mensagem = ex.Message });
                
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: " + ExceptionUtil.ExibeDetalhes(ex) });
            }
        }
    }
}
