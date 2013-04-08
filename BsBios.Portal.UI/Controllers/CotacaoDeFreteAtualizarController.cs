using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoDeFreteAtualizarController : Controller
    {
        private readonly IAtualizadorDeCotacaoDeFrete _atualizadorDeCotacao;

        public CotacaoDeFreteAtualizarController(IAtualizadorDeCotacaoDeFrete atualizadorDeCotacao)
        {
            _atualizadorDeCotacao = atualizadorDeCotacao;
        }

        [HttpPost]
        public JsonResult Salvar(CotacaoInformarVm cotacaoInformarVm)
        {
            try
            {
                _atualizadorDeCotacao.Atualizar(cotacaoInformarVm);
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }

        }
    }

}
