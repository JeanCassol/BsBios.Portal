using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoDeFreteServiceController : Controller
    {
        private readonly IProcessoDeCotacaoDeFreteService _processoDeCotacaoDeFreteService;

        public ProcessoDeCotacaoDeFreteServiceController(IProcessoDeCotacaoDeFreteService processoDeCotacaoDeFreteService)
        {
            _processoDeCotacaoDeFreteService = processoDeCotacaoDeFreteService;
        }

        [HttpPost]
        public JsonResult Salvar(ProcessoCotacaoFreteCadastroVm processoCotacaoFreteCadastroVm)
        {
            try
            {
                _processoDeCotacaoDeFreteService.Salvar(processoCotacaoFreteCadastroVm);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {

                return Json(new {Sucesso = false, Mensagem = ex.Message});
            }
            
        }

    }
}
