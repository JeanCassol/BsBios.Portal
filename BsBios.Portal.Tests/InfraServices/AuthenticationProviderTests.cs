using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.IoC;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.InfraServices
{
    [TestClass]
    public class AuthenticationProviderTests
    {
        [TestInitialize]
        public void Inicializar()
        {
            _authenticationProvider = ObjectFactory.GetInstance<IAuthenticationProvider>();
        }

        private IAuthenticationProvider _authenticationProvider;

        [TestMethod]
        public void QuandoMeAutenticarComUmCompradorDeveRetornarPerfilComprador()
        {
            UsuarioConectado usuarioConectado = _authenticationProvider.Autenticar("comprador", "123");
            Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilComprador));
        }

        [TestMethod]
        public void QuandoMeAutenticarComUmFornecedorDeveRetornarPerfilFornecedor()
        {
            UsuarioConectado usuarioConectado = _authenticationProvider.Autenticar("fornecedor", "123");
            Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilFornecedor));
        }

        [TestMethod]
        public void QuandoMeAutenticarComCredenciaisInvalidasDeveRetornarPerfilNaoAutorizado()
        {
            UsuarioConectado usuarioConectado = _authenticationProvider.Autenticar("comprador", "1234");
            Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilNaoAutorizado));
        }
    }
}
