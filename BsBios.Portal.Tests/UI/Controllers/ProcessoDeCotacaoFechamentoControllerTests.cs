using System.ComponentModel;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoDeCotacaoFechamentoControllerTests
    {
        private readonly ProcessoDeCotacaoFechamentoVm _processoDeCotacaoFechamentoVm = new ProcessoDeCotacaoFechamentoVm
            {
                IdProcessoCotacao = 10,
                Justificativa = "justificativa"
            };
        #region testes do fechamento do processo de cotação de frete
        [TestMethod]
        public void QuandoFecharProcessoDeCotacaoDeFreteCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>()));
            var serviceFactoryMock = new Mock<IFechamentoDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            serviceFactoryMock.Setup(x => x.Construir()).Returns(serviceMock.Object);

            var processoDeCotacaoController = new ProcessoDeCotacaoDeFreteFechamentoController(serviceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(_processoDeCotacaoFechamentoVm);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi fechado com sucesso.", props.Find("Mensagem", true).GetValue(data));

            serviceFactoryMock.Verify(x => x.Construir(), Times.Once());
            serviceMock.Verify(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOcorrerErroAoFecharProcessoDeCotacaoDeFreteDeveRetornarMensagemDeErro()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>())).Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado.")); ;
            var serviceFactoryMock = new Mock<IFechamentoDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            serviceFactoryMock.Setup(x => x.Construir()).Returns(serviceMock.Object); 

            var processoDeCotacaoController = new ProcessoDeCotacaoDeFreteFechamentoController(serviceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(_processoDeCotacaoFechamentoVm);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: Processo XXXXX não encontrado.",
                props.Find("Mensagem", true).GetValue(data));

        }
        #endregion

        #region testes do fechamento do processo de cotação de material
        [TestMethod]
        public void QuandoFecharProcessoDeCotacaoDeMaterialCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>()));
            var serviceFactoryMock = new Mock<IFechamentoDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            serviceFactoryMock.Setup(x => x.Construir()).Returns(serviceMock.Object);

            var processoDeCotacaoController = new ProcessoDeCotacaoDeMaterialFechamentoController(serviceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(_processoDeCotacaoFechamentoVm);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi fechado com sucesso.", props.Find("Mensagem", true).GetValue(data));

            serviceFactoryMock.Verify(x => x.Construir(), Times.Once());
            serviceMock.Verify(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOcorrerErroAoFecharProcessoDeCotacaoDeMaterialDeveRetornarMensagemDeErro()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoFechamentoVm>())).Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado.")); ;
            var serviceFactoryMock = new Mock<IFechamentoDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            serviceFactoryMock.Setup(x => x.Construir()).Returns(serviceMock.Object);

            var processoDeCotacaoController = new ProcessoDeCotacaoDeMaterialFechamentoController(serviceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(_processoDeCotacaoFechamentoVm);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: Processo XXXXX não encontrado.",
                props.Find("Mensagem", true).GetValue(data));

        }
        #endregion
    }
}
