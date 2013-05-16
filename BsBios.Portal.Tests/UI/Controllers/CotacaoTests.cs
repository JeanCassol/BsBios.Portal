﻿using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class CotacaoTests
    {
        private readonly CotacaoMaterialController _controller;
        private static readonly UsuarioConectado UsuarioConectado = ObjectFactory.GetInstance<UsuarioConectado>();
        private readonly Mock<IConsultaCotacaoDoFornecedor> _consultaCotacaoDoFornecedorMock;
        private readonly Mock<IConsultaCondicaoPagamento> _consultaCondicaoPagamentoMock;
        private readonly Mock<IConsultaIncoterm> _consultaIncotermsMock;
        private readonly Mock<IConsultaTipoDeFrete> _consultaTipoDeFreteMock;
        private readonly Mock<IAtualizadorDeIteracaoDoUsuario> _atualizadorDeIteracaoDoUsuarioMock;

        public CotacaoTests()
        {
            _consultaCotacaoDoFornecedorMock = new Mock<IConsultaCotacaoDoFornecedor>(MockBehavior.Strict);
            _consultaCotacaoDoFornecedorMock.Setup(x => x.ConsultarCotacaoDeMaterial(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new CotacaoMaterialCadastroVm());

            _consultaCondicaoPagamentoMock = new Mock<IConsultaCondicaoPagamento>(MockBehavior.Strict);
            _consultaCondicaoPagamentoMock.Setup(x => x.ListarTodas())
                .Returns(new List<CondicaoDePagamentoCadastroVm>());

            _consultaIncotermsMock = new Mock<IConsultaIncoterm>(MockBehavior.Strict);
            _consultaIncotermsMock.Setup(x => x.ListarTodos())
                .Returns(new List<IncotermCadastroVm>());

            _consultaTipoDeFreteMock= new Mock<IConsultaTipoDeFrete>(MockBehavior.Strict);
            _consultaTipoDeFreteMock.Setup(x => x.Listar())
                                    .Returns(new List<TipoDeFreteVm>());

            _atualizadorDeIteracaoDoUsuarioMock = new Mock<IAtualizadorDeIteracaoDoUsuario>(MockBehavior.Strict);
            _atualizadorDeIteracaoDoUsuarioMock.Setup(x => x.Atualizar(It.IsAny<int>()));

            _controller = new CotacaoMaterialController(_consultaCotacaoDoFornecedorMock.Object,
                _consultaCondicaoPagamentoMock.Object, _consultaIncotermsMock.Object,_atualizadorDeIteracaoDoUsuarioMock.Object, _consultaTipoDeFreteMock.Object);
        }

        [TestMethod]
        public void QuandoEditarACotacaoRetornaAViewCorretaComModelo()
        {
            ViewResult viewResult = _controller.EditarCadastro(10);
            Assert.AreEqual("Cadastro", viewResult.ViewName);
            Assert.IsInstanceOfType(viewResult.Model, typeof(CotacaoMaterialCadastroVm));

            _consultaCotacaoDoFornecedorMock.Verify(x => x.ConsultarCotacaoDeMaterial(It.IsAny<int>(), It.IsAny<string>()), Times.Once());

        }

        [TestMethod]
        public void QuandoEditarACotacaoRealizaAConsultaPassandoOUsuarioLogado()
        {
            _consultaCotacaoDoFornecedorMock.Setup(x => x.ConsultarCotacaoDeMaterial(It.IsAny<int>(), It.IsAny<string>()))
                .Returns(new CotacaoMaterialCadastroVm())
                .Callback((int idProcessoCotacao, string login) => Assert.AreEqual(UsuarioConectado.Login, login));

            _controller.EditarCadastro(10);
        }

        [TestMethod]
        public void QuandoEditarACotacaoRegistraQueOFornecedorVisualizouOProcessoDeCotacao()
        {
            _controller.EditarCadastro(10);
            _atualizadorDeIteracaoDoUsuarioMock.Verify(x => x.Atualizar(It.IsAny<int>()), Times.Once());
        }
    }
}
