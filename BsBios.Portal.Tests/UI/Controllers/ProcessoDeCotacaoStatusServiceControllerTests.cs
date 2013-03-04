using System.ComponentModel;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoDeCotacaoStatusServiceControllerTests
    {
        [TestMethod]
        public void QuandoAbrirProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeCotacaoStatusServiceMock = new Mock<IProcessoDeCotacaoStatusService>(MockBehavior.Strict);
            processoDeCotacaoStatusServiceMock.Setup(x => x.AbrirProcesso(It.IsAny<int>()));
            var processoDeCotacaoController = new ProcessoDeCotacaoStatusServiceController(processoDeCotacaoStatusServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.AbrirProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi aberto com sucesso.", props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoStatusServiceMock.Verify(x=> x.AbrirProcesso(It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public void QuandoOcorrerErroAoAbrirProcessoDeCotacaoDeveRetornarMensagemDeErro()
        {
            var processoDeCotacaoStatusServiceMock = new Mock<IProcessoDeCotacaoStatusService>(MockBehavior.Strict);
            processoDeCotacaoStatusServiceMock.Setup(x => x.AbrirProcesso(It.IsAny<int>()))
                .Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado."));
            var controller = new ProcessoDeCotacaoStatusServiceController(processoDeCotacaoStatusServiceMock.Object);
            JsonResult retorno = controller.AbrirProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao abrir o Processo de Cotação. Detalhes: Processo XXXXX não encontrado.", 
                props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoStatusServiceMock.Verify(x => x.AbrirProcesso(It.IsAny<int>()), Times.Once());
        }

        [TestMethod]
        public void QuandoFecharProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeCotacaoStatusServiceMock = new Mock<IProcessoDeCotacaoStatusService>(MockBehavior.Strict);
            processoDeCotacaoStatusServiceMock.Setup(x => x.FecharProcesso(It.IsAny<int>()));
            var processoDeCotacaoController = new ProcessoDeCotacaoStatusServiceController(processoDeCotacaoStatusServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi fechado com sucesso.", props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoStatusServiceMock.Verify(x => x.FecharProcesso(It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public void QuandoOcorrerErroAoFecharProcessoDeCotacaoDeveRetornarMensagemDeErro()
        {
            var processoDeCotacaoStatusServiceMock = new Mock<IProcessoDeCotacaoStatusService>(MockBehavior.Strict);
            processoDeCotacaoStatusServiceMock.Setup(x => x.FecharProcesso(It.IsAny<int>()))
                .Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado."));
            var controller = new ProcessoDeCotacaoStatusServiceController(processoDeCotacaoStatusServiceMock.Object);
            JsonResult retorno = controller.FecharProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao fechar o Processo de Cotação. Detalhes: Processo XXXXX não encontrado.",
                props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoStatusServiceMock.Verify(x => x.FecharProcesso(It.IsAny<int>()), Times.Once());
        }

    }
}
