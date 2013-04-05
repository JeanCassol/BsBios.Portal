using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoDeCotacaoDeFreteEmailController : Controller
    {
        private readonly IReenviadorDeEmailDoProcessoDeCotacao _service;

        public ProcessoDeCotacaoDeFreteEmailController(IReenviadorDeEmailDoProcessoDeCotacaoFactory serviceFactory)
        {
            _service = serviceFactory.Construir();
        }

        [HttpGet]
        public JsonResult EnviarEmailDeAbertura(int idProcessoCotacao, int idFornecedorParticipante)
        {
            try
            {
                _service.ReenviarEmailDeAbertura(idProcessoCotacao,idFornecedorParticipante);
                return Json(new { Sucesso = true, Mensagem = "O e-mail foi enviado com sucesso." }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = "Ocorreu um erro ao enviar o e-mail para o Fornecedor. Detalhes: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
