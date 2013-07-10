using System.ComponentModel;
using System.Web.Mvc;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoDeCotacaoDeMaterialFechamentoControllerTests
    {
        private readonly ProcessoDeCotacaoDeMaterialFechamentoInfoVm _processoDeCotacaoFechamentoVm = new ProcessoDeCotacaoDeMaterialFechamentoInfoVm
            {
                IdProcessoCotacao = 10,
                TextoDeCabecalho = "justificativa",
                NotaDeCabecalho = "nota de cabeçalho",
                DocumentoParaGerarNoSap = (int) Enumeradores.DocumentoDoSap.Pedido
            };

        #region testes do fechamento do processo de cotação de material
        [TestMethod]
        public void QuandoFecharProcessoDeCotacaoDeMaterialCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoDeMaterialFechamentoInfoVm>()));

            var processoDeCotacaoController = new ProcessoDeCotacaoDeMaterialFechamentoController(serviceMock.Object);
            JsonResult retorno = processoDeCotacaoController.FecharProcesso(_processoDeCotacaoFechamentoVm);
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("O Processo de Cotação foi fechado com sucesso.", props.Find("Mensagem", true).GetValue(data));

            serviceMock.Verify(x => x.Executar(It.IsAny<ProcessoDeCotacaoDeMaterialFechamentoInfoVm>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOcorrerErroAoFecharProcessoDeCotacaoDeMaterialDeveRetornarMensagemDeErro()
        {
            var serviceMock = new Mock<IFechamentoDeProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            serviceMock.Setup(x => x.Executar(It.IsAny<ProcessoDeCotacaoDeMaterialFechamentoInfoVm>())).Throws(new ExcecaoDeTeste("Processo XXXXX não encontrado.")); ;
            var serviceFactoryMock = new Mock<IFechamentoDeProcessoDeCotacaoServiceFactory>(MockBehavior.Strict);

            var processoDeCotacaoController = new ProcessoDeCotacaoDeMaterialFechamentoController(serviceMock.Object);
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
