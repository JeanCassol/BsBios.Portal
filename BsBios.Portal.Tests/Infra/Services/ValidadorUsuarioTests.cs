using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class ValidadorUsuarioTests
    {
        private readonly IValidadorUsuario _validadorUsuario;
        private readonly Mock<IUsuarios> _usuariosMock;
        private readonly IProvedorDeCriptografia _provedorDeCriptografia;

        private readonly IList<Usuario> _usuariosCadastrados;



        public ValidadorUsuarioTests()
        {
            _provedorDeCriptografia = ObjectFactory.GetInstance<IProvedorDeCriptografia>();
            _usuariosCadastrados = new List<Usuario>();
            
            var usuarioCompradorLogistica = new Usuario("Comprador Logistica", "logistica", null);
            usuarioCompradorLogistica.CriarSenha(_provedorDeCriptografia.Criptografar("123"));
            usuarioCompradorLogistica.AdicionarPerfil(Enumeradores.Perfil.CompradorLogistica);
            _usuariosCadastrados.Add(usuarioCompradorLogistica);

            var usuarioSemSenha = new Usuario("Sem Senha", "semsenha", null);
            usuarioSemSenha.AdicionarPerfil(Enumeradores.Perfil.CompradorSuprimentos);
            _usuariosCadastrados.Add(usuarioSemSenha);

            var usuarioBloqueado = new Usuario("Usuário Bloqueado", "bloqueado", null);
            usuarioBloqueado.CriarSenha(_provedorDeCriptografia.Criptografar("123"));
            usuarioBloqueado.AdicionarPerfil(Enumeradores.Perfil.Fornecedor);
            usuarioBloqueado.Bloquear();
            _usuariosCadastrados.Add(usuarioBloqueado);

            var usuarioSemPerfil = new Usuario("sem perfil", "semperfil", null);
            usuarioSemPerfil.CriarSenha(_provedorDeCriptografia.Criptografar("123"));
            _usuariosCadastrados.Add(usuarioSemPerfil);

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);
            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>()))
                         .Returns((string login) => _usuariosCadastrados.SingleOrDefault(x => x.Login == login));
            
            _validadorUsuario = new ValidadorUsuario(_usuariosMock.Object, _provedorDeCriptografia);

        }

        [TestMethod]
        public void ConsigoMeConectarComCredenciaisValidas()
        {
            UsuarioConectado usuarioConectado =  _validadorUsuario.Validar("logistica", "123");
            Assert.IsNotNull(usuarioConectado);
            Assert.AreEqual("logistica", usuarioConectado.Login);
            Assert.AreEqual(1, usuarioConectado.Perfis.Count(x => x == Enumeradores.Perfil.CompradorLogistica));

            _usuariosMock.Verify(x => x.BuscaPorLogin(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(SenhaIncorretaException))]
        public void QuandoConectarComSenhaIncorretaDeveRetornarExcecao()
        {
            _validadorUsuario.Validar("logistica", "456");
        }

        [TestMethod]
        [ExpectedException(typeof(UsuarioNaoCadastradoException))]
        public void QuandoConectarComUsuarioInexistenteDeveRetornarExcecao()
        {
            _validadorUsuario.Validar("master", "123");
        }

        [TestMethod]
        [ExpectedException(typeof(UsuarioSemSenhaException))]
        public void NaoConsigoMeAutenticarComUsuarioSemSenha()
        {
            _validadorUsuario.Validar("semsenha", "123");
        }

        [TestMethod]
        [ExpectedException(typeof(UsuarioBloqueadoException))]
        public void NaoConsigoMeAutenticarComUsuarioBloqueado()
        {
            _validadorUsuario.Validar("bloqueado", "123");
        }

        [TestMethod]
        [ExpectedException(typeof(UsuarioSemPerfilException))]
        public void NaoConsigoMeAutenticarComUsuarioSemPerfilAssociado()
        {
            _validadorUsuario.Validar("semperfil", "123");
        }

    }
}
