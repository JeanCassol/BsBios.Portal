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
        public JsonResult AtualizarProcesso(ProcessoDeCotacaoAtualizarVm atualizacaoDoProcessoDeCotacaoVm)
        {
            try
            {
                var idProcessoCotacao = _processoDeCotacaoService.AtualizarProcesso(atualizacaoDoProcessoDeCotacaoVm);
                return Json(new { Sucesso = true, IdProcessoCotacao = idProcessoCotacao });

                //_processoDeCotacaoService.AtualizarProcesso(atualizacaoDoProcessoDeCotacaoVm);
                //return RedirectToAction("Index", "ProcessoDeCotacaoDeMaterial");
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});

                //ViewData["erro"] = ex.Message;
                //return RedirectToAction("EditarCadastro", "ProcessoDeCotacaoDeMaterial", new { idProcessoCotacao = atualizacaoDoProcessoDeCotacaoVm.Id });
            }
            
        }

        [HttpGet]
        public JsonResult VerificarQuantidadeAdquirida(int idProcessoCotacao, int idItem, decimal quantidadeAdquiridaTotal)
        {
            try
            {
                VerificacaoDeQuantidadeAdquiridaVm verificacaoVm = _processoDeCotacaoService.VerificarQuantidadeAdquirida(idProcessoCotacao, idItem, quantidadeAdquiridaTotal);
                return Json(new {Sucesso = true, Verificacao = verificacaoVm}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
