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
    public class ProcessoDeCotacaoFornecedoresServiceControllerTests
    {
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeCotacaoFornecedoresServiceMock = new Mock<IProcessoDeCotacaoFornecedoresService>(MockBehavior.Strict);
            processoDeCotacaoFornecedoresServiceMock.Setup(x => x.AtualizarFornecedores(It.IsAny<ProcessoDeCotacaoFornecedoresAtualizarVm>()));
            var processoDeCotacaoController = new ProcessoDeCotacaoFornecedoresServiceController(processoDeCotacaoFornecedoresServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.AtualizarFornecedores(new ProcessoDeCotacaoFornecedoresAtualizarVm());
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Atualização dos Fornecedores do Processo de Cotação realizada com sucesso.", props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoFornecedoresServiceMock.Verify(x=> x.AtualizarFornecedores(It.IsAny<ProcessoDeCotacaoFornecedoresAtualizarVm>()), Times.Once());
        }
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoComErroDeveRetornarMensagemDeErro()
        {
            var processoDeCotacaoFornecedoresServiceMock = new Mock<IProcessoDeCotacaoFornecedoresService>(MockBehavior.Strict);
            processoDeCotacaoFornecedoresServiceMock.Setup(x => x.AtualizarFornecedores(It.IsAny<ProcessoDeCotacaoFornecedoresAtualizarVm>()))
                .Throws(new ExcecaoDeTeste("Fornecedor XXXXX não encontrado."));
            var processoDeCotacaoController = new ProcessoDeCotacaoFornecedoresServiceController(processoDeCotacaoFornecedoresServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.AtualizarFornecedores(new ProcessoDeCotacaoFornecedoresAtualizarVm());
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao atualizar os fornecedores do Processo de Cotação. Detalhes: Fornecedor XXXXX não encontrado.", 
                props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoFornecedoresServiceMock.Verify(x => x.AtualizarFornecedores(It.IsAny<ProcessoDeCotacaoFornecedoresAtualizarVm>()), Times.Once());
        }
    }
}
