using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Hosting;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class UsuarioApiTests
    {
        [TestMethod]
        public void QuandoAtualizarUmaListaDeUsuariosComSucessoDeveRetornarStatusOk()
        {
            var cadastroUsuarioMock = new Mock<ICadastroUsuario>(MockBehavior.Strict);
            cadastroUsuarioMock.Setup(x => x.AtualizarUsuarios(It.IsAny<IList<UsuarioCadastroVm>>()));
            var usuarioApiController = new UsuarioApiController(cadastroUsuarioMock.Object);
            var usuarioCadastroVm = new UsuarioCadastroVm()
            {
                Login = "USER001",
                Nome = "USUARIO 001",
                Email = "usuario001@empresa.com.br"
            };
            usuarioApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/UsuarioApi/PostMultiplo");
            usuarioApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = usuarioApiController.PostMultiplo(new ListaUsuario(){ usuarioCadastroVm });

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            cadastroUsuarioMock.Verify(x => x.AtualizarUsuarios(It.IsAny<IList<UsuarioCadastroVm>>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorrerErroAoAtualizarUmaListaDeUsuariosDeveRetornarStatusDeErro()
        {
            var cadastroUsuarioMock = new Mock<ICadastroUsuario>(MockBehavior.Strict);
            cadastroUsuarioMock.Setup(x => x.AtualizarUsuarios(It.IsAny<IList<UsuarioCadastroVm>>()))
                .Throws(new Exception("Ocorreu um erro ao atualizar os Ivas"));

            var usuarioApiController = new UsuarioApiController(cadastroUsuarioMock.Object);
            var usuarioCadastroVm = new UsuarioCadastroVm()
            {
                Login = "USER001",
                Nome = "USUARIO 001",
                Email = "usuario001@empresa.com.br"
            };
            usuarioApiController.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/UsuarioApi/PostMultiplo");
            usuarioApiController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var resposta = usuarioApiController.PostMultiplo(new ListaUsuario() { usuarioCadastroVm });
            var apiResponseMessage = (ApiResponseMessage)((ObjectContent)(resposta.Content)).Value;

            Assert.AreEqual(HttpStatusCode.OK, resposta.StatusCode);
            Assert.AreEqual("500", apiResponseMessage.Retorno.Codigo);
            cadastroUsuarioMock.Verify(x => x.AtualizarUsuarios(It.IsAny<IList<UsuarioCadastroVm>>()), Times.Once());
        }
    }
}
