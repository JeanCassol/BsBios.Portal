using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class OrdemDeTransporteController : Controller
    {
        private readonly IConsultaOrdemDeTransporte _consultaOrdemDeTransporte ;

        public OrdemDeTransporteController(IConsultaOrdemDeTransporte consultaOrdemDeTransporte)
        {
            _consultaOrdemDeTransporte = consultaOrdemDeTransporte;
        }

        //
        // GET: /OrdemDeTransporte/
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacao, OrdemDeTransporteListagemFiltroVm filtro)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                filtro.CodigoDoFornecedor = usuarioConectado.Login;
            }
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
        public ActionResult NovaColeta()
        {
            var coletaVm = new ColetaVm
            {
                PermiteEditar = true

            };
            return PartialView("Coleta", coletaVm);
        }

        [HttpGet]
        public ActionResult EditarColeta(int idDaOrdemDeTransporte, int idDaColeta)
        {
            ColetaVm coletaVm = _consultaOrdemDeTransporte.ConsultaColeta(idDaOrdemDeTransporte, idDaColeta);

            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            coletaVm.PermiteEditar = usuarioConectado.PermiteAlterarColeta();
            
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
    }
}
