using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoServiceController : Controller
    {
        private readonly IProcessoDeCotacaoService _processoDeCotacaoService;

        public ProcessoDeCotacaoServiceController(IProcessoDeCotacaoService processoDeCotacaoService)
        {
            _processoDeCotacaoService = processoDeCotacaoService;
        }

        [HttpPost]
        public ActionResult AtualizarProcesso(ProcessoDeCotacaoAtualizarVm atualizacaoDoProcessoDeCotacaoVm)
        {
            try
            {
                _processoDeCotacaoService.AtualizarProcesso(atualizacaoDoProcessoDeCotacaoVm);
                return RedirectToAction("Index", "ProcessoCotacaoMaterial");
            }
            catch (Exception ex)
            {
                ViewData["erro"] = ex.Message;
                return RedirectToAction("EditarCadastro", "ProcessoCotacaoMaterial", new { idProcessoCotacao = atualizacaoDoProcessoDeCotacaoVm.Id });
            }
            
        }

    }
}
