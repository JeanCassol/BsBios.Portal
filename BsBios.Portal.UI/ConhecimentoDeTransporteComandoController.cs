using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI
{
    [SecurityFilter]
    public class ConhecimentoDeTransporteComandoController : BaseController
    {

        private readonly IConhecimentoDeTransporteComando _conhecimentoDeTransporteComando;
        private readonly ICadastroDeConhecimentoDeTransporte _cadastroDeConhecimentoDeTransporte;

        public ConhecimentoDeTransporteComandoController(IConhecimentoDeTransporteComando conhecimentoDeTransporteComando, ICadastroDeConhecimentoDeTransporte cadastroDeConhecimentoDeTransporte)
        {
            _conhecimentoDeTransporteComando = conhecimentoDeTransporteComando;
            _cadastroDeConhecimentoDeTransporte = cadastroDeConhecimentoDeTransporte;
        }

        [HttpPost]
        public ActionResult AtribuirOrdemDeTransporte(string chaveDoConhecimento, int idDaOrdemDeTransporte)
        {
            try
            {
                _conhecimentoDeTransporteComando.AtribuirOrdemDeTransporte(chaveDoConhecimento, idDaOrdemDeTransporte);
                return Json(new {Sucesso = true});

            }
            catch (Exception exception)
            {
                return Json(new {Sucesso = false, Mensagem = exception.Message});
            }
        }

        [HttpPost]
        public ActionResult Reprocessar()
        {
            try
            {
                _cadastroDeConhecimentoDeTransporte.Reprocessar();
                return Json(new {Sucesso = true});
            }
            catch (Exception exception)
            {
                return Json(new {Sucesso = false, Mensagem = exception.Message});
            }
        }
    }
}
