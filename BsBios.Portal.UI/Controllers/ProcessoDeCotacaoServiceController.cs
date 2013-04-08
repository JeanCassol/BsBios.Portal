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
        private readonly IProcessoDeCotacaoDeMaterialService _processoDeCotacaoService;

        public ProcessoDeCotacaoServiceController(IProcessoDeCotacaoDeMaterialService processoDeCotacaoService)
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

        [HttpGet]
        public JsonResult VerificarQuantidadeAdquirida(int idProcessoCotacao, decimal quantidadeAdquiridaTotal)
        {
            try
            {
                VerificacaoDeQuantidadeAdquiridaVm verificacaoVm = _processoDeCotacaoService.VerificarQuantidadeAdquirida(idProcessoCotacao, quantidadeAdquiridaTotal);
                return Json(new {Sucesso = true, Verificacao = verificacaoVm}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
