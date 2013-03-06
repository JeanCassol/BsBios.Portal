using System;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class CotacaoTests
    {
        private readonly CotacaoController _controller;
        private readonly UsuarioConectado _usuarioConectado;
        private readonly Mock<IConsultaCotacaoDoFornecedor> _consultaCotacaoDoFornecedorMock;

        public CotacaoTests()
        {
            _usuarioConectado = DefaultObjects.ObtemUsuarioConectado();
            _consultaCotacaoDoFornecedorMock = new Mock<IConsultaCotacaoDoFornecedor>(MockBehavior.Strict);
            _consultaCotacaoDoFornecedorMock.Setup(x => x.ConsultarCotacao(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new CotacaoCadastroVm());
            _controller = new CotacaoController(_consultaCotacaoDoFornecedorMock.Object,_usuarioConectado);
        }

        [TestMethod]
        public void QuandoEditarACotacaoRetornaAViewCorretaComModelo()
        {
            ViewResult viewResult = _controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(CotacaoCadastroVm));

            _consultaCotacaoDoFornecedorMock.Verify(x => x.ConsultarCotacao(It.IsAny<int>(), It.IsAny<string>()), Times.Once());

        }

        [TestMethod]
        public void QuandoEditarACotacaoRealizaAConsultaPassandoOUsuarioLogado()
        {
            _consultaCotacaoDoFornecedorMock.Setup(x => x.ConsultarCotacao(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new CotacaoCadastroVm())
                .Callback((int idProcessoCotacao, string login) => Assert.AreEqual(_usuarioConectado.Login, login));

            _controller.EditarCadastro(10);
        }
    }
}
