using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using Microsoft.Ajax.Utilities;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class OrdemDeTransporteController : Controller
    {
        private readonly IConsultaOrdemDeTransporte _consultaOrdemDeTransporte ;
        private readonly IConsultaTerminal _consultaTerminal;

        public OrdemDeTransporteController(IConsultaOrdemDeTransporte consultaOrdemDeTransporte, IConsultaTerminal consultaTerminal)
        {
            _consultaOrdemDeTransporte = consultaOrdemDeTransporte;
            _consultaTerminal = consultaTerminal;
        }

        //
        // GET: /OrdemDeTransporte/
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Terminais = _consultaTerminal.ListarTodos();
            return View();
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacao, OrdemDeTransporteListagemFiltroVm filtro)
        {
            KendoGridVm kendoGridVm = _consultaOrdemDeTransporte.Listar(paginacao, filtro);
            return Json(new { registros = kendoGridVm.Registros, totalCount = kendoGridVm.QuantidadeDeRegistros }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Cadastro(int id)
        {
            OrdemDeTransporteCadastroVm cadastro = _consultaOrdemDeTransporte.ConsultarOrdem(id);
            return View("Cadastro",cadastro);
        }

        [HttpGet]
        public JsonResult ListarColetas(PaginacaoVm paginacaoVm, int idDaOrdemDeTransporte)
        {

            var kendoGridVm = _consultaOrdemDeTransporte.ListarColetas(paginacaoVm, idDaOrdemDeTransporte);

            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NovaColeta(string unidadeDeMedida)
        {
            var coletaVm = new ColetaVm
            {
                PermiteEditar = true,
                UnidadeDeMedida = unidadeDeMedida
            };
            return PartialView("Coleta", coletaVm);
        }

        [HttpGet]
        public ActionResult EditarColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            ColetaVm coletaVm = _consultaOrdemDeTransporte.ConsultaColeta(idDaOrdemDeTransporte, idDaColeta,usuarioConectado);

           
            return PartialView("Coleta", coletaVm);
        }


        public JsonResult NotasFiscaisDaColeta(int idDaOrdemDeTransporte, int idColeta)
        {
            try
            {
                IList<NotaFiscalDeColetaVm> notasFiscais = _consultaOrdemDeTransporte.NotasFiscaisDaColeta(idDaOrdemDeTransporte, idColeta);
                return Json(new { Sucesso = true, NotasFiscais = notasFiscais }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { Sucesso = false, Mensagem = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult RealizarColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            ColetaVm coletaVm = _consultaOrdemDeTransporte.ConsultaColeta(idDaOrdemDeTransporte, idDaColeta,usuarioConectado);

            coletaVm.PermiteRealizar = true;
    
            return PartialView("Coleta", coletaVm);
        }
    }
}
