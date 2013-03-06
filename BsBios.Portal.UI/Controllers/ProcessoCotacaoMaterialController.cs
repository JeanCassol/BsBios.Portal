using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class ProcessoCotacaoMaterialController : Controller
    {
        private readonly IConsultaProcessoDeCotacaoDeMaterial _consultaProcessoDeCotacaoDeMaterial;
        private readonly UsuarioConectado _usuarioConectado;

        public ProcessoCotacaoMaterialController(IConsultaProcessoDeCotacaoDeMaterial consultaProcessoDeCotacaoDeMaterial, UsuarioConectado usuarioConectado)
        {
            _consultaProcessoDeCotacaoDeMaterial = consultaProcessoDeCotacaoDeMaterial;
            _usuarioConectado = usuarioConectado;
        }


        [HttpGet]
        public ActionResult Index()
        {
            if (_usuarioConectado.Perfil == (int) Common.Enumeradores.Perfil.Comprador)
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro","ProcessoCotacaoMaterial");

            }
            if (_usuarioConectado.Perfil == (int) Common.Enumeradores.Perfil.Fornecedor)
            {
                ViewData["ActionEdicao"] = Url.Action("EditarCadastro", "Cotacao");
            }

            return View();
        }
        [HttpGet]
        public JsonResult Listar(PaginacaoVm paginacaoVm)
        {
            var filtro = new ProcessoCotacaoMaterialFiltroVm();
            if (_usuarioConectado.Perfil == (int) Common.Enumeradores.Perfil.Fornecedor)
            {
                filtro.CodigoFornecedor = _usuarioConectado.Login;
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

        public ActionResult SelecionarFornecedores(int idProcessoCotacao, string codigoProduto)
        {
            ViewData["CodigoProduto"] = codigoProduto;
            ViewData["IdProcessoCotacao"] = idProcessoCotacao; 
            return PartialView("_SelecionarFornecedor");
        }

    }
}
