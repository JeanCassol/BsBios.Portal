using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.UI.Controllers
{
    public class CotacaoController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;
        private readonly UsuarioConectado _usuarioConectado;

        public CotacaoController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor, UsuarioConectado usuarioConectado)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
            _usuarioConectado = usuarioConectado;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            CotacaoCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacao(idProcessoCotacao, _usuarioConectado.Login);
            return View("Cadastro",viewModel);
        }

    }
}
