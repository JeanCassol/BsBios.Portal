using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
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
            CotacaoFreteCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacaoDeFrete(idProcessoCotacao, usuarioConectado.Login);
            return View("Cadastro",viewModel);
        }
    }
}
