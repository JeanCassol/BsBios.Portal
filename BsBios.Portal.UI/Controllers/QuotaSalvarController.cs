using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class QuotaSalvarController : Controller
    {
        private readonly ICadastroQuota _cadastroQuota;

        public QuotaSalvarController(ICadastroQuota cadastroQuota)
        {
            _cadastroQuota = cadastroQuota;
        }

        [HttpPost]
        public JsonResult Salvar(IList<QuotaSalvarVm> quotas)
        {
            try
            {
                _cadastroQuota.Salvar(quotas);
                return Json(new {Sucesso = true});
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message});
            }
        }
    }
}