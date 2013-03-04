using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class ProcessoDeCotacaoController : Controller
    {
        private readonly IProcessoDeCotacaoService _processoDeCotacaoService;

        public ProcessoDeCotacaoController(IProcessoDeCotacaoService processoDeCotacaoService)
        {
            _processoDeCotacaoService = processoDeCotacaoService;
        }

        [HttpPost]
        public JsonResult AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm)
        {
            try
            {
                _processoDeCotacaoService.AtualizarFornecedores(atualizacaoDosFornecedoresVm);
                return Json(new { Sucesso = true, Mensagem = "Atualização dos Fornecedores do Processo de Cotação realizada com sucesso." });

            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao atualizar os fornecedores do Processo de Cotação. Detalhes: " + ex.Message });
            }
        }

    }
}
