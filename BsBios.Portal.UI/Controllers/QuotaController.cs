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
        private readonly IConsultaTerminal _consultarTerminal;

        public QuotaController(IConsultaQuota consultaQuota, IConsultaFluxoDeCarga consultaFluxoDeCarga, IConsultaMaterialDeCarga consultaMaterialDeCarga, IConsultaTerminal consultarTerminal)
        {
            _consultaQuota = consultaQuota;
            _consultaFluxoDeCarga = consultaFluxoDeCarga;
            _consultaMaterialDeCarga = consultaMaterialDeCarga;
            _consultarTerminal = consultarTerminal;
        }

        [HttpGet]
        public ActionResult Cadastro()
        {
            ViewBag.FluxosDeCarga = _consultaFluxoDeCarga.Listar();
            ViewBag.MateriaisDeCarga = _consultaMaterialDeCarga.Listar();
            ViewBag.Terminais = _consultarTerminal.ListarTodos();
            return View();
        }

        [HttpGet]
        public JsonResult ListarFornecedores(DateTime dataDaQuota, string codigoDoTerminal)
        {
            try
            {
                IList<QuotaConsultarVm> quotas = _consultaQuota.QuotasDaData(dataDaQuota, codigoDoTerminal);
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

        [HttpGet]
        public JsonResult NotasFiscaisDoAgendamento(int idQuota, int idAgendamento)
        {
            try
            {
                IList<NotaFiscalVm> notasFiscais = _consultaQuota.NotasFiscaisDoAgendamento(idQuota, idAgendamento);
                return Json(new {Sucesso = true, NotasFiscais = notasFiscais}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }

    }
}
