using System;
using System.Collections.Generic;
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
            return View("Cadastro",cadastro);
        }

        [HttpGet]
        public JsonResult ListarColetas()
        {

            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            bool permiteEditar = usuarioConectado.PermiteAlterarColeta();

            var coleta = new ColetaListagemVm
            {
                Id = 1,
                DataDePrevisaoDeChegada = DateTime.Now.Date.AddDays(2).ToShortDateString(),
                Motorista = "Mauro Leal",
                Placa = "IOQ-5338",
                Peso = 150,
                PermiteEditar = permiteEditar

            };

            var kendoGridVm = new KendoGridVm
            {
                QuantidadeDeRegistros = 1,
                Registros = new List<ListagemVm> {coleta}
            };

            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult NovaColeta()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            
            var coletaVm = new ColetaVm {PermiteEditar = usuarioConectado.PermiteAlterarColeta()};
            return PartialView("Coleta", coletaVm);
        }

        [HttpGet]
        public ActionResult EditarColeta()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();

            var coletaVm = new ColetaVm
            {
                PermiteEditar = usuarioConectado.PermiteAlterarColeta(),
                Id = 1,
                DataDePrevisaoDeChegada = new DateTime(2013,11,18).ToShortDateString(),
                Motorista = "Mauro Leal",
                Placa = "IOQ-5339",
                Peso = 80,
                ValorDoFrete = 1789
                
            };
            
            return PartialView("Coleta", coletaVm);
        }



    }
}
