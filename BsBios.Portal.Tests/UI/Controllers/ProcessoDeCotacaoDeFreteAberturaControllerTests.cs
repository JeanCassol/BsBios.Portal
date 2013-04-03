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
    public class ProcessoDeCotacaoDeFreteAberturaControllerTests
    {
        [TestMethod]
        public void QuandoAbrirProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeAberturaServiceMock = new Mock<IAberturaDeProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeAberturaServiceMock.Setup(x => x.Executar(It.IsAny<int>()));
            var processoDeAberturaServiceFactoryMock = new Mock<IAberturaDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            processoDeAberturaServiceFactoryMock.Setup(x => x.Construir()).Returns(processoDeAberturaServiceMock.Object);
            var processoDeCotacaoController = new ProcessoDeCotacaoDeFreteAberturaController(processoDeAberturaServiceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.AbrirProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi aberto com sucesso.", props.Find("Mensagem", true).GetValue(data));

            processoDeAberturaServiceFactoryMock.Verify(x => x.Construir(), Times.Once());
            processoDeAberturaServiceMock.Verify(x => x.Executar(It.IsAny<int>()), Times.Once());
        }
        [TestMethod]
        public void QuandoOcorrerErroAoAbrirProcessoDeCotacaoDeveRetornarMensagemDeErro()
        {
            var processoDeAberturaServiceMock = new Mock<IAberturaDeProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeAberturaServiceMock.Setup(x => x.Executar(It.IsAny<int>())).Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado."));
            var processoDeAberturaServiceFactoryMock = new Mock<IAberturaDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);
            processoDeAberturaServiceFactoryMock.Setup(x => x.Construir()).Returns(processoDeAberturaServiceMock.Object);

            var processoDeCotacaoController = new ProcessoDeCotacaoDeFreteAberturaController(processoDeAberturaServiceFactoryMock.Object);
            JsonResult retorno = processoDeCotacaoController.AbrirProcesso(10);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao abrir o Processo de Cotação. Detalhes: Processo XXXXX não encontrado.",
                props.Find("Mensagem", true).GetValue(data));

        }
    }
}
