using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class UsuarioController : Controller
    {
        private readonly IConsultaUsuario _consultaUsuario;

        public UsuarioController(IConsultaUsuario consultaUsuario)
        {
            _consultaUsuario = consultaUsuario;
        }

        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }
        [HttpGet]
        public ViewResult EditarCadastro(string login)
        {
            UsuarioConsultaVm usuarioConsultaVm = _consultaUsuario.ConsultaPorLogin(login);
            return View("Cadastro", usuarioConsultaVm);
        }

        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, UsuarioFiltroVm usuarioFiltroVm)
        {
            KendoGridVm kendoGridVm = _consultaUsuario.Listar(paginacaoVm, usuarioFiltroVm);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult PerfisDoUsuario(string login)
        {
            try
            {
                IList<PerfilVm> perfis = _consultaUsuario.PerfisDoUsuario(login);
                return Json(new {Sucesso = true, Registros = perfis}, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new {Sucesso = false, Mensagem = ex.Message}, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
