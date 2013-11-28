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
    public class ProcessoDeCotacaoDeMaterialController : Controller
    {
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacaoDeMaterial;
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;

        public ProcessoDeCotacaoDeMaterialController(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial, IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao)
        {
            _consultaProcessoDeCotacaoDeMaterial = consultaProcessoDeCotacaoDeMaterial;
            _consultaStatusProcessoCotacao = consultaStatusProcessoCotacao;
        }


        [HttpGet]
        public ActionResult Index()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.CompradorSuprimentos))
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro","ProcessoDeCotacaoDeMaterial");
            }
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro", "CotacaoMaterial");
            }

            ViewData["ActionListagem"] = Url.Action("Listar","ProcessoDeCotacaoDeMaterial");
            ViewBag.TituloDaPagina = "Cotações de Material";
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            return View("_ProcessoCotacaoIndex");
        }
        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, ProcessoDeCotacaoDeFreteFiltroVm filtro)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            filtro.TipoDeCotacao = (int) Enumeradores.TipoDeCotacao.Material;
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                filtro.CodigoFornecedor = usuarioConectado.Login;
            }

            var kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.Listar(paginacaoVm, filtro);
            return Json(new { registros = kendoGridVm.Registros, totalCount = kendoGridVm.QuantidadeDeRegistros }, JsonRequestBehavior.AllowGet);
        }

        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            try
            {
                ProcessoCotacaoMaterialCadastroVm cadastro = _consultaProcessoDeCotacaoDeMaterial.ConsultaProcesso(idProcessoCotacao);
                return View("Cadastro", cadastro);
            }
            catch (Exception ex)
            {
                ViewData["erro"] = ex.Message;
                return View("Cadastro");
            }
        }

        [HttpGet]
        public JsonResult ListarFornecedores(int idProcessoCotacao)
        {
            var kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.FornecedoresParticipantes(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SelecionarFornecedores(int idProcessoCotacao, string codigoProduto, Enumeradores.TipoDeCotacao tipoDeCotacao)
        {
            ViewData["CodigoProduto"] = codigoProduto;
            ViewData["IdProcessoCotacao"] = idProcessoCotacao;
            ViewData["TipoDeCotacao"] = tipoDeCotacao;
            return PartialView("_SelecionarFornecedores");
        }

        public PartialViewResult SelecionarCotacoes(int idProcessoCotacao)
        {
            try
            {
                ViewData["IdProcessoCotacao"] = idProcessoCotacao;
                return PartialView("_SelecionarCotacao");

            }
            catch (Exception ex)
            {
                ViewData["IdProcessoCotacao"] = idProcessoCotacao;
                ViewData["erro"] = ex.Message;
                return PartialView("_SelecionarCotacao");
            }
        }

        [HttpGet]
        public JsonResult ListarCotacoes(int idProcessoCotacao)
        {
            IList<CotacaoMaterialSelecionarVm> cotacoes = _consultaProcessoDeCotacaoDeMaterial.CotacoesDosFornecedores(idProcessoCotacao);
            return Json(new {Registros = cotacoes}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCotacoesResumido(int idProcessoCotacao)
        {
            KendoGridVm kendoGridVm  = _consultaProcessoDeCotacaoDeMaterial.CotacoesDosFornecedoresResumido(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }


    }
}
