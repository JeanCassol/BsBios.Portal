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
    public class ProcessoDeCotacaoServiceControllerTests
    {
        [TestMethod]
        public void QuandoAtualizaOProcessoComSucessoRecebeMensagemDeSucesso()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()))
                .Returns(1);

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            var retorno = processoDeCotacaoController.AtualizarProcesso(new ProcessoDeCotacaoAtualizarVm());

            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));

            processoDeCotacaoServiceMock.Verify(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorreErroAoAtualizarProcessoRetornarParaPaginaDeCadastro()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()))
                .Throws(new ExcecaoDeTeste("Erro ao Atualizar Processo de Cotação"));

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            var retorno = processoDeCotacaoController.AtualizarProcesso(
                new ProcessoDeCotacaoAtualizarVm()
                    {
                        Id = 10,
                        DataLimiteRetorno = DateTime.Today
                    });

            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Erro ao Atualizar Processo de Cotação",props.Find("Mensagem",true).GetValue(data));

            processoDeCotacaoServiceMock.Verify(x => x.AtualizarProcesso(It.IsAny<ProcessoDeCotacaoAtualizarVm>()), Times.Once());
            
        }

        [TestMethod]
        public void QuandoCompararQuantidadeAdquiridaComSucessoRetornaResultadoDaComparacao()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.VerificarQuantidadeAdquirida(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<decimal>()))
                                        .Returns(new VerificacaoDeQuantidadeAdquiridaVm
                                            {
                                                QuantidadeSolicitadaNoProcessoDeCotacao = 1000,
                                                SuperouQuantidadeSolicitada = false
                                            });

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.VerificarQuantidadeAdquirida(10,1, 1000);

            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsTrue(props.Find("Sucesso", true).GetValue(data));
            VerificacaoDeQuantidadeAdquiridaVm verificacaoVm = props.Find("Verificacao", true).GetValue(data);
            Assert.IsNotNull(verificacaoVm);
            Assert.AreEqual(1000, verificacaoVm.QuantidadeSolicitadaNoProcessoDeCotacao);
            Assert.IsFalse(verificacaoVm.SuperouQuantidadeSolicitada);

        }

        [TestMethod]
        public void QuandoCompararQuantidadeAdquiridaComErroRetornaMensagemDeErro()
        {
            var processoDeCotacaoServiceMock = new Mock<IProcessoDeCotacaoDeMaterialService>(MockBehavior.Strict);
            processoDeCotacaoServiceMock.Setup(x => x.VerificarQuantidadeAdquirida(It.IsAny<int>(),It.IsAny<int>(), It.IsAny<decimal>()))
                                        .Throws(new ExcecaoDeTeste("Erro ao comparar quantidade adquirida."));

            var processoDeCotacaoController = new ProcessoDeCotacaoServiceController(processoDeCotacaoServiceMock.Object);
            JsonResult retorno = processoDeCotacaoController.VerificarQuantidadeAdquirida(10,1, 1000);

            dynamic data = retorno.Data;
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(retorno.Data);

            Assert.IsFalse(props.Find("Sucesso", true).GetValue(data));
            Assert.AreEqual("Erro ao comparar quantidade adquirida.", props.Find("Mensagem", true).GetValue(data));

        }


    }
}
