using System;
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
    public class ProcessoDeCotacaoControllerTests
    {
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarFornecedores(It.IsAny<AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm>()));
            var processoDeCotacaoController = new ProcessoDeCotacaoController(processoDeCotacaoServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.AtualizarFornecedores(new AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm());
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Atualização dos Fornecedores do Processo de Cotação realizada com sucesso.", props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoServiceMock.Verify(x=> x.AtualizarFornecedores(It.IsAny<AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm>()), Times.Once());
        }
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoComErroDeveRetornarMensagemDeErro()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarFornecedores(It.IsAny<AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm>()))
                .Throws(new ExcecaoDeTeste("Fornecedor XXXXX não encontrado."));
            var processoDeCotacaoController = new ProcessoDeCotacaoController(processoDeCotacaoServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.AtualizarFornecedores(new AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm());
            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Ocorreu um erro ao atualizar os fornecedores do Processo de Cotação. Detalhes: Fornecedor XXXXX não encontrado.", 
                props.Find("Mensagem", true).GetValue(data));

            processoDeCotacaoServiceMock.Verify(x => x.AtualizarFornecedores(It.IsAny<AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm>()), Times.Once());
        }
    }
}
