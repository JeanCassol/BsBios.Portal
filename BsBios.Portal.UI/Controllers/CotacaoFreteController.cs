using System;
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
    public class CotacaoFreteController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;

        public CotacaoFreteController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            CotacaoCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacao(idProcessoCotacao, usuarioConectado.Login);
            ViewData["TipoDeCotacao"] = Enumeradores.TipoDeCotacao.Frete;
            return View("_CotacaoCadastro",viewModel);
        }


    }
}
