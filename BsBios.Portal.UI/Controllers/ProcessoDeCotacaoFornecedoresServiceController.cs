using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class ProcessoDeCotacaoFornecedoresServiceController : Controller
    {
        private readonly IProcessoDeCotacaoFornecedoresService _processoDeCotacaoFornecedoresService;

        public ProcessoDeCotacaoFornecedoresServiceController(IProcessoDeCotacaoFornecedoresService processoDeCotacaoFornecedoresService)
        {
            _processoDeCotacaoFornecedoresService = processoDeCotacaoFornecedoresService;
        }

        [HttpPost]
        public JsonResult AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm)
        {
            try
            {
                _processoDeCotacaoFornecedoresService.AtualizarFornecedores(atualizacaoDosFornecedoresVm);
                return Json(new { Sucesso = true, Mensagem = "Atualização dos Fornecedores do Processo de Cotação realizada com sucesso." });

            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao atualizar os fornecedores do Processo de Cotação. Detalhes: " + ex.Message });
            }
        }

    }
}
