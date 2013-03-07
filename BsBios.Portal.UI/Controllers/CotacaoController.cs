using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;
        private readonly IConsultaCondicaoPagamento _consultaCondicaoPagamento;
        private readonly IConsultaIncoterms _consultaIncoterms;

        public CotacaoController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor, 
            IConsultaCondicaoPagamento consultaCondicaoPagamento, IConsultaIncoterms consultaIncoterms)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
            _consultaCondicaoPagamento = consultaCondicaoPagamento;
            _consultaIncoterms = consultaIncoterms;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            ViewBag.Incoterms = _consultaIncoterms.ListarTodos();
            ViewBag.CondicoesDePagamento = _consultaCondicaoPagamento.ListarTodas();
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            CotacaoCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacao(idProcessoCotacao, usuarioConectado.Login);
            return View("Cadastro",viewModel);
        }


    }
}
