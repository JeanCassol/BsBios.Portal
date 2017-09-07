using System;
using System.Web.Mvc;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ConfiguracaoController : Controller
    {
        private readonly IServicoDeConfiguracao _servicoDeConfiguracao;

        public ConfiguracaoController(IServicoDeConfiguracao servicoDeConfiguracao)
        {
            _servicoDeConfiguracao = servicoDeConfiguracao;
        }

        public JsonResult Consultar()
        {
            try
            {
                Configuracao configuracao = _servicoDeConfiguracao.ObterConfiguracao();
                return Json(new {Sucesso = true, configuracao}, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message},JsonRequestBehavior.AllowGet);
            }
        }

    }
}
