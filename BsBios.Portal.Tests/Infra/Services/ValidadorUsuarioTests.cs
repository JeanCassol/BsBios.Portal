using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class ValidadorUsuarioTests
    {
        private static IValidadorUsuario _validadorUsuario;
        private static ICadastroUsuario _cadastroUsuario;

        [ClassInitialize]
        public static void  Inicializar(TestContext context)
        {
            _validadorUsuario = ObjectFactory.GetInstance<IValidadorUsuario>();

            Queries.RemoverUsuariosCadastrados();
            _cadastroUsuario = ObjectFactory.GetInstance<ICadastroUsuario>();
            var usuarioCompradorVm =
            new UsuarioCadastroVm()
                {
                    //CodigoPerfil = 1,
                    Nome = "Usuário Comprador",
                    Login = "comprador",
                    //Senha = "123",
                    Email = "comprador@bsbios.com"
                };
            _cadastroUsuario.Novo(usuarioCompradorVm);

            _cadastroUsuario.CriarSenha("comprador", "123");

            var usuarioFornecedorVm = new UsuarioCadastroVm()
                {
                    //CodigoPerfil = 2,
                    Nome = "Usuário Fornecedor",
                    Login = "fornecedor",
                    //Senha = "456",
                    Email = "fornecedor@transportadora.com.br"
                };

            _cadastroUsuario.Novo(usuarioFornecedorVm);

        }


        [TestMethod]
        public void QuandoMeAutenticarComUmCompradorDeveRetornarPerfilComprador()
        {
            UsuarioConectado usuarioConectado = _validadorUsuario.Validar("comprador", "123");
            Assert.AreEqual(Enumeradores.Perfil.Comprador, (Enumeradores.Perfil) usuarioConectado.Perfil);
        }

        [TestMethod]
        public void QuandoMeAutenticarComUmFornecedorDeveRetornarPerfilFornecedor()
        {
            UsuarioConectado usuarioConectado = _validadorUsuario.Validar("fornecedor", "123");
            Assert.AreEqual(Enumeradores.Perfil.Fornecedor, (Enumeradores.Perfil)usuarioConectado.Perfil);
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
            _validadorUsuario.Validar("master", "123");
        }

        [TestMethod]
        [ExpectedException(typeof(SenhaIncorretaException))]
        public void QuandoConectarComSenhaIncorretaDeveRetornarExcecao()
        {
            _validadorUsuario.Validar("comprador", "456");
        }

        [TestMethod]
        public void NaoConsigoMeAutenticarComUsuarioSemSenha()
        {
            Assert.Fail();   
        }

        [TestMethod]
        public void NaoConsigoMeAutenticarComUsuarioDesabilitado()
        {
            Assert.Fail();   
        }

    }
}
