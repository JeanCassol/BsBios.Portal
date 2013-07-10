using System.Web.Mvc;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Filters;
using BsBios.Portal.ViewModel;
using StructureMap;

namespace BsBios.Portal.UI.Controllers
{
    [SecurityFilter]
    public class CotacaoFreteController : Controller
    {
        private readonly IConsultaCotacaoDoFornecedor _consultaCotacaoDoFornecedor;
        private readonly IAtualizadorDeIteracaoDoUsuario _atualizadorDeIteracaoDoUsuario;


        public CotacaoFreteController(IConsultaCotacaoDoFornecedor consultaCotacaoDoFornecedor, 
            IAtualizadorDeIteracaoDoUsuario atualizadorDeIteracaoDoUsuario)
        {
            _consultaCotacaoDoFornecedor = consultaCotacaoDoFornecedor;
            _atualizadorDeIteracaoDoUsuario = atualizadorDeIteracaoDoUsuario;
        }

        [HttpGet]
        public ViewResult EditarCadastro(int idProcessoCotacao)
        {
            var usuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
            CotacaoFreteCadastroVm viewModel = _consultaCotacaoDoFornecedor.ConsultarCotacaoDeFrete(idProcessoCotacao, usuarioConectado.Login);
            _atualizadorDeIteracaoDoUsuario.Atualizar(viewModel.IdFornecedorParticipante);

            return View("Cadastro",viewModel);
        }
    }
}
