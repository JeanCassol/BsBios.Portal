using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Controllers.ModelBinders;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoDeMaterialAtualizarController : Controller
    {
        private readonly IAtualizadorDeCotacaoDeMaterial _atualizadorDeCotacao;

        public CotacaoDeMaterialAtualizarController(IAtualizadorDeCotacaoDeMaterial atualizadorDeCotacao)
        {
            _atualizadorDeCotacao = atualizadorDeCotacao;
        }

        [HttpPost]
        public JsonResult Salvar(CotacaoMaterialInformarVm cotacaoInformarVm,[ModelBinder(typeof(CotacaoImpostoModelBinder))] CotacaoImpostosVm cotacaoImpostosVm)
        {
            try
            {
                _atualizadorDeCotacao.AtualizarCotacao(cotacaoInformarVm);
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult SalvarItem(CotacaoMaterialItemInformarVm cotacaoItemInformarVm, [ModelBinder(typeof (CotacaoImpostoModelBinder))] CotacaoImpostosVm cotacaoImpostosVm)
        {
            cotacaoItemInformarVm.Impostos = cotacaoImpostosVm;
            try
            {
                _atualizadorDeCotacao.AtualizarItemDaCotacao(cotacaoItemInformarVm);
                return Json(new { Sucesso = true });
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message });
            }
            
        }
    }

}
