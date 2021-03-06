﻿using System;
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
    public class ProcessoCotacaoFreteController : Controller
    {
        private readonly IConsultaUnidadeDeMedida _consultaUnidadeDeMedida ;
        private readonly IConsultaProcessoDeCotacaoDeFrete _consultaProcessoDeCotacaoDeFrete;
        private readonly IConsultaStatusProcessoCotacao _consultaStatusProcessoCotacao;

        public ProcessoCotacaoFreteController(IConsultaUnidadeDeMedida consultaUnidadeDeMedida,
            IConsultaProcessoDeCotacaoDeFrete consultaProcessoDeCotacaoDeFrete, 
            IConsultaStatusProcessoCotacao consultaStatusProcessoCotacao)
        {
            _consultaUnidadeDeMedida = consultaUnidadeDeMedida;
            _consultaProcessoDeCotacaoDeFrete = consultaProcessoDeCotacaoDeFrete;
            _consultaStatusProcessoCotacao = consultaStatusProcessoCotacao;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.CompradorLogistica))
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro", "ProcessoCotacaoFrete");
            }
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro", "CotacaoFrete");
            }

            ViewData["ActionListagem"] = Url.Action("Listar", "ProcessoCotacaoFrete");
            ViewBag.TituloDaPagina = "Cotações de Frete";
            ViewBag.StatusProcessoCotacao = _consultaStatusProcessoCotacao.Listar();
            ViewBag.TipoDeCotacao = Enumeradores.TipoDeCotacao.Frete;
            return View("_ProcessoCotacaoIndex");
        }
        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            
            if (usuarioConectado.Perfis.Contains(Enumeradores.Perfil.Fornecedor))
            {
                filtro.CodigoFornecedor = usuarioConectado.Login;
            }

            //var kendoGridVm = _consultaProcessoDeCotacaoDeMaterial.Listar(paginacaoVm, filtro);
            var kendoGridVm = _consultaProcessoDeCotacaoDeFrete.Listar(paginacaoVm, filtro);
            return Json(new { registros = kendoGridVm.Registros, totalCount = kendoGridVm.QuantidadeDeRegistros }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ViewResult NovoCadastro()
        {
            ViewBag.UnidadesDeMedida = _consultaUnidadeDeMedida.ListarTodos();
            
            return View("Cadastro");
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            try
            {
                ProcessoCotacaoFreteCadastroVm cadastro = _consultaProcessoDeCotacaoDeFrete.ConsultaProcesso(idProcessoCotacao);
                ViewBag.UnidadesDeMedida = _consultaUnidadeDeMedida.ListarTodos();
                
                return View("Cadastro", cadastro);

            }
            catch (Exception ex)
            {
                ViewData["erro"] = ex.Message;
                return View("Cadastro");
            }

        }

        public ActionResult SelecionarProduto(ProdutoCadastroVm produtoCadastroVm)
        {
            return PartialView("_SelecionarProduto", produtoCadastroVm);
        }

        public ActionResult SelecionarItinerario(ItinerarioCadastroVm itinerarioCadastroVm)
        {
            return View("_SelecionarItinerario", itinerarioCadastroVm);
        }

        public JsonResult ListarCotacoes(int idProcessoCotacao)
        {
            IList<CotacaoSelecionarVm> cotacoes = _consultaProcessoDeCotacaoDeFrete.CotacoesDosFornecedores(idProcessoCotacao);
            return Json(new { Registros = cotacoes }, JsonRequestBehavior.AllowGet);
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

        public JsonResult ListarCotacoesResumido(int idProcessoCotacao)
        {
            KendoGridVm kendoGridVm = _consultaProcessoDeCotacaoDeFrete.CotacoesDosFornecedoresResumido(idProcessoCotacao);
            return Json(kendoGridVm, JsonRequestBehavior.AllowGet);
        }
    }
}
