using System;
using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoCotacaoMaterialController : Controller
    {
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacaoDeMaterial;
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;

        public ProcessoCotacaoMaterialController(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial, IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao)
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
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro","ProcessoCotacaoMaterial");
            }
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro", "CotacaoMaterial");
            }

            ViewData["ActionListagem"] = Url.Action("Listar","ProcessoCotacaoMaterial");
            ViewBag.TituloDaPagina = "Cotações de Material";
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            ViewBag.TipoDeCotacao = Enumeradores.TipoDeCotacao.Material;
            return View("_ProcessoCotacaoIndex");
        }
        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro)
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

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            return View("Cadastro");
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

        public ActionResult SelecionarFornecedores(int idProcessoCotacao, Enumeradores.TipoDeCotacao tipoDeCotacao)
        {
            ViewData["IdProcessoCotacao"] = idProcessoCotacao;
            ViewData["TipoDeCotacao"] = tipoDeCotacao;
            return PartialView("_SelecionarFornecedores");
        }

        public ActionResult SelecionarItens()
        {
            return PartialView("_SelecionarItens");
        }

        public PartialViewResult SelecionarCotacoes(int idProcessoCotacaoItem)
        {
            try
            {
                ViewData["IdProcessoCotacaoItem"] = idProcessoCotacaoItem;
                return PartialView("_SelecionarCotacao");

            }
            catch (Exception ex)
            {
                ViewData["IdProcessoCotacaoItem"] = idProcessoCotacaoItem;
                ViewData["erro"] = ex.Message;
                return PartialView("_SelecionarCotacao");
            }
        }

        [HttpGet]
        public JsonResult ListarCotacoes(int idProcessoCotacao, int idProcessoCotacaoItem)
        {
            IList<CotacaoMaterialSelecionarVm> cotacoes = _consultaProcessoDeCotacaoDeMaterial.CotacoesDosFornecedores(idProcessoCotacao, idProcessoCotacaoItem);
            return Json(new {Registros = cotacoes}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarCotacoesResumido(int idProcessoCotacao)
        {
            KendoGridVm kendoGridVm  = _consultaProcessoDeCotacaoDeMaterial.CotacoesDosFornecedoresResumido(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ListarItens(int idProcessoCotacao)
        {
            KendoGridVm kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.ListarItens(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FecharProcesso()
        {
            return PartialView("_FecharProcesso");
        }
    }
}
