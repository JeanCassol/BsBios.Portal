using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
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
        public JsonResult FecharProcesso(int idProcessoCotacao, Enumeradores.TipoDeCotacao tipoDeCotacao)
        {
            try
            {
                IComunicacaoSap comunicacaoSap = null;
                if (tipoDeCotacao == Enumeradores.TipoDeCotacao.Frete)
                {
                    comunicacaoSap = new ComunicacaoFechamentoProcessoCotacaoFrete();
                }
                if (tipoDeCotacao == Enumeradores.TipoDeCotacao.Material)
                {
                    comunicacaoSap = new ComunicacaoFechamentoProcessoCotacaoMaterial();
                }
                _processoDeCotacaoStatusService.ComunicacaoSap = comunicacaoSap;
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
