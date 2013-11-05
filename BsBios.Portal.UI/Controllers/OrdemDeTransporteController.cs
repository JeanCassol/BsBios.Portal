using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
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
            return View("Cadastro");
        }

        public ActionResult Salvar()
        {
            throw new NotImplementedException();
        }
    }
}
