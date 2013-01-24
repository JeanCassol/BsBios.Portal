using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class ValidadorUsuarioTests
    {
        [TestInitialize]
        public void Inicializar()
        {
            _authenticationProvider = ObjectFactory.GetInstance<IValidadorUsuario>();
        }

        private IValidadorUsuario _authenticationProvider;

        [TestMethod]
        public void QuandoMeAutenticarComUmCompradorDeveRetornarPerfilComprador()
        {
            UsuarioConectado usuarioConectado = _authenticationProvider.Validar("comprador", "123");
            Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilComprador));
        }

        [TestMethod]
        public void QuandoMeAutenticarComUmFornecedorDeveRetornarPerfilFornecedor()
        {
            UsuarioConectado usuarioConectado = _authenticationProvider.Validar("fornecedor", "123");
            Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilFornecedor));
        }

        //[TestMethod]
        //public void QuandoMeAutenticarComCredenciaisInvalidasDeveRetornarPerfilNaoAutorizado()
        //{
        //    UsuarioConectado usuarioConectado = _authenticationProvider.Validar("comprador", "1234");
        //    Assert.IsInstanceOfType(usuarioConectado.Perfil, typeof(PerfilNaoAutorizado));
        //}
        [TestMethod]
        [ExpectedException(typeof(UsuarioNaoCadastradoException))]
        public void QuandoConectarComUsuarioInexistenteDeveRetornarExcecao()
        {
        }

        [TestMethod]
        [ExpectedException(typeof(SenhaIncorretaException))]
        public void QuandoConectarComSenhaIncorretaDeveRetornarExcecao()
        {
        }


    }
}
