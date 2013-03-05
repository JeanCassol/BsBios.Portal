using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
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
