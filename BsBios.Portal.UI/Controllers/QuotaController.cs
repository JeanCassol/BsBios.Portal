using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class QuotaController : Controller
    {
        private readonly IConsultaQuota _consultaQuota;
        private readonly IConsultaFluxoDeCarga _consultaFluxoDeCarga;
        private readonly IConsultaMaterialDeCarga _consultaMaterialDeCarga;

        public QuotaController(IConsultaQuota consultaQuota, IConsultaFluxoDeCarga consultaFluxoDeCarga, IConsultaMaterialDeCarga consultaMaterialDeCarga)
        {
            _consultaQuota = consultaQuota;
            _consultaFluxoDeCarga = consultaFluxoDeCarga;
            _consultaMaterialDeCarga = consultaMaterialDeCarga;
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            ViewBag.FluxosDeCarga = _consultaFluxoDeCarga.Listar();
            ViewBag.MateriaisDeCarga = _consultaMaterialDeCarga.Listar();
            return View(new QuotaCadastroVm{Terminal = "1000"});
        }

        [HttpGet]
        public JsonResult ListarFornecedores(DateTime dataDaQuota)
        {
            try
            {
                IList<QuotaConsultarVm> quotas = _consultaQuota.QuotasDaData(dataDaQuota);
                return Json(new {Sucesso = true, Registros = quotas}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult ListarQuotasPorFornecedor(PaginacaoVm paginacaoVm)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            return Json(_consultaQuota.ListarQuotasDoFornecedor(paginacaoVm,usuarioConectado.Login), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult QuotaPorFornecedor()
        {
            return View();
        }

    }
}
