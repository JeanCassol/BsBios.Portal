using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CadastroUsuarioOperacaoTests
    {
        [TestMethod]
        public void QuandoCriarNovoUsuarioDeveRetornarMesmasPropriedades()
        {
            var usuarioCadastroVm = new UsuarioCadastroVm()
                {
                    Login = "USER001",
                    Email = "user01@empresa.com.br",
                    Nome = "USUARIO 001"
                };

            var cadastroUsuarioOperacao = new CadastroUsuarioOperacao();
            var usuario = cadastroUsuarioOperacao.Criar(usuarioCadastroVm);
            Assert.AreEqual("USER001", usuario.Login);
            Assert.AreEqual("USUARIO 001", usuario.Nome);
            Assert.AreEqual("user01@empresa.com.br", usuario.Email);
        }

        [TestMethod]
        public void QuandoAtualizarIvaDeveAtualizarAsPropriedades()
        {
            var usuario = new Usuario("USUARIO 001","USER001", "user001@empresa.com.br",Enumeradores.Perfil.Comprador);

            var usuarioCadastroVm = new UsuarioCadastroVm()
            {
                Login = "USER001",
                Nome = "USUARIO 001 ALTERADO",
                Email = "usuario001alterado@empresa.com.br"
            };

            var cadastroUsuarioOperacao = new CadastroUsuarioOperacao();
            cadastroUsuarioOperacao.Alterar(usuario, usuarioCadastroVm);

            Assert.AreEqual("USER001", usuario.Login);
            Assert.AreEqual("USUARIO 001 ALTERADO", usuario.Nome);
            Assert.AreEqual("usuario001alterado@empresa.com.br",usuario.Email);

        }
    }
}
