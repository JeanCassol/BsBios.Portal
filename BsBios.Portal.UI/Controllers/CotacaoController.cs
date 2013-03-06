using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;
        private readonly IConsultaCondicaoPagamento _consultaCondicaoPagamento;
        private readonly IConsultaIncoterms _consultaIncoterms;
        private readonly UsuarioConectado _usuarioConectado;

        public CotacaoController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor, UsuarioConectado usuarioConectado, 
            IConsultaCondicaoPagamento consultaCondicaoPagamento, IConsultaIncoterms consultaIncoterms)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
            _usuarioConectado = usuarioConectado;
            _consultaCondicaoPagamento = consultaCondicaoPagamento;
            _consultaIncoterms = consultaIncoterms;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            ViewBag.Incoterms = _consultaIncoterms.ListarTodos();
            ViewBag.CondicoesDePagamento = _consultaCondicaoPagamento.ListarTodas();
            CotacaoCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacao(idProcessoCotacao, _usuarioConectado.Login);
            return View("Cadastro",viewModel);
        }


    }
}
