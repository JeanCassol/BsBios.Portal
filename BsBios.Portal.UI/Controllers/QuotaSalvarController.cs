using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using log4net;
using Newtonsoft.Json;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class QuotaSalvarController : Controller
    {
        private readonly ICadastroQuota _cadastroQuota;
        private readonly IFormatadorDeLog _formatadorDeLog;
        private static readonly ILog Log = LogManager.GetLogger("Quota");

        public QuotaSalvarController(ICadastroQuota cadastroQuota, IFormatadorDeLog formatadorDeLog)
        {
            _cadastroQuota = cadastroQuota;
            _formatadorDeLog = formatadorDeLog;
        }

        [HttpPost]
        [JsonFilter(Param = "quotas", JsonDataType = typeof(QuotasSalvarVm))]
        public JsonResult Salvar(QuotasSalvarVm quotas)
        {
            try
            {
                Log.Info($"{_formatadorDeLog.FormatarUsuario()} - Requisicao Salvar Quota - {JsonConvert.SerializeObject(quotas)}");
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