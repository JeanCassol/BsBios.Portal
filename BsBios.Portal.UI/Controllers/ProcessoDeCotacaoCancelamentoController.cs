using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoCancelamentoController : Controller
    {
        private readonly ICancelamentoDeProcessoDeCotacaoService _service;

        public ProcessoDeCotacaoCancelamentoController(ICancelamentoDeProcessoDeCotacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResult CancelarProcesso(int idProcessoCotacao)
        {
            try
            {
                _service.Executar(idProcessoCotacao);
                return Json(new { Sucesso = true, Mensagem = "O Processo de Cotação foi cancelado com sucesso." });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao cancelar o Processo de Cotação. Detalhes: " + ex.Message });
            }
        }
    }
}
