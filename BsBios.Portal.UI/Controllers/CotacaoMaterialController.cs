using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoMaterialController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;
        private readonly IConsultaCondicaoPagamento _consultaCondicaoPagamento;
        private readonly IConsultaIncoterm _consultaIncoterms;
        private readonly IConsultaTipoDeFrete _consultaTipoDeFrete;
        private readonly IAtualizadorDeIteracaoDoUsuario _atualizadorDeIteracaoDoUsuario;

        
        public CotacaoMaterialController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor, IConsultaCondicaoPagamento consultaCondicaoPagamento, 
            IConsultaIncoterm consultaIncoterms, IAtualizadorDeIteracaoDoUsuario atualizadorDeIteracaoDoUsuario, IConsultaTipoDeFrete consultaTipoDeFrete)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
            _consultaCondicaoPagamento = consultaCondicaoPagamento;
            _consultaIncoterms = consultaIncoterms;
            _atualizadorDeIteracaoDoUsuario = atualizadorDeIteracaoDoUsuario;
            _consultaTipoDeFrete = consultaTipoDeFrete;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            CotacaoMaterialCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacaoDeMaterial(idProcessoCotacao, usuarioConectado.Login);
            _atualizadorDeIteracaoDoUsuario.Atualizar(viewModel.IdFornecedorParticipante);
            ViewBag.Incoterms = _consultaIncoterms.ListarTodos();
            ViewBag.CondicoesDePagamento = _consultaCondicaoPagamento.ListarTodas();
            ViewBag.TiposDeFrete = _consultaTipoDeFrete.Listar();
            return View(viewModel);
        }

        public ActionResult ConsultarCadastro(int idProcessoCotacao, string codigoFornecedor)
        {
            CotacaoMaterialConsultarCadastroVm vm = _consultaCotacaoDoFornecedor.ConsultarCotacaoDeMaterialParaVisualizacao(idProcessoCotacao, codigoFornecedor);
            return PartialView("_ConsultarCadastro",vm);
        }
    }
}
