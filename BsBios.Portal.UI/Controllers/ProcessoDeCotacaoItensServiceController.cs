using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoItensServiceController : Controller
    {
        private readonly IProcessoDeCotacaoDeMaterialItensService _service;

        public ProcessoDeCotacaoItensServiceController(IProcessoDeCotacaoDeMaterialItensService service)
        {
            _service = service;
        }

        [HttpPost]
        public JsonResult AtualizarItens(int idProcessoCotacao, IList<int> idsDasRequisicoesDeCompra)
        {
            try
            {
                _service.AtualizarItens(idProcessoCotacao, idsDasRequisicoesDeCompra);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});

            }
        }
    }
}