using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class GeradorDeEmailDeFechamentoDeProcessoDeCotacaoTests
    {
        private readonly IGeradorDeEmailDeFechamentoDeProcessoDeCotacao _geradorDeEmail;
        private readonly Mock<IEmailService> _emailServiceMock;
        private readonly Mock<IGeradorDeMensagemDeEmail> _geradorDeMensagemDeEmailMock;

        public GeradorDeEmailDeFechamentoDeProcessoDeCotacaoTests()
        {
            _geradorDeMensagemDeEmailMock = new Mock<IGeradorDeMensagemDeEmail>(MockBehavior.Strict);
            _geradorDeMensagemDeEmailMock.Setup(
                x => x.FornecedoresSelecionadosNoProcessoDeCotacao(It.IsAny<ProcessoDeCotacao>(), It.IsAny<Cotacao>())).Returns(new MensagemDeEmail("assunto", "conteudo"));
            _geradorDeMensagemDeEmailMock.Setup(
                x =>
                    x.FornecedoresSelecionadosNoProcessoDeCotacaoDeFrete(It.IsAny<ProcessoDeCotacaoDeFrete>(),
                        It.IsAny<Cotacao>()))
                .Returns(new MensagemDeEmail("assunto", "conteudo"));
            _geradorDeMensagemDeEmailMock.Setup(
                x => x.FornecedoresNaoSelecionadosNoProcessoDeCotacao(It.IsAny<ProcessoDeCotacao>()))
                                         .Returns(new MensagemDeEmail("assunto", "conteudo"));

            _geradorDeMensagemDeEmailMock.Setup(x => x.AutorizacaoDeTransporte(It.IsAny<ProcessoDeCotacaoDeFrete>()))
                .Returns(new MensagemDeEmail("autorizacao", "conteudo"));

            _emailServiceMock = new Mock<IEmailService>(MockBehavior.Strict);
            _emailServiceMock.Setup(x => x.Enviar(It.IsAny<string>(), It.IsAny<MensagemDeEmail>())).Returns(true);
            _geradorDeEmail = new GeradorDeEmailDeFechamentoDeProcessoDeCotacao(_geradorDeMensagemDeEmailMock.Object,_emailServiceMock.Object);
        }

        [TestMethod]
        public void QuandoFornecedorNaoPreencheuACotacaoNaoEnviaEmailAoFecharProcessoDeCotacao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComFornecedor();
            _geradorDeEmail.GerarEmail(processoDeCotacao);
            _emailServiceMock.Verify(x => x.Enviar(It.IsAny<string>(), It.IsAny<MensagemDeEmail>()), Times.Never());
           
        }

        [TestMethod]
        public void QuandoCotacaoForSelecionadaEnviaEmailDeCotacaoSelecionada()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            _geradorDeEmail.GerarEmail(processoDeCotacao);
            _emailServiceMock.Verify(x => x.Enviar(It.IsAny<string>(), It.IsAny<MensagemDeEmail>()), Times.Once());
            _geradorDeMensagemDeEmailMock.Verify(x => x.FornecedoresSelecionadosNoProcessoDeCotacao(It.IsAny<ProcessoDeCotacao>(),It.IsAny<Cotacao>() ), Times.Once());
        }
        [TestMethod]
        public void QuandoCotacaoNaoForSelecionadaEnviaEmailDeCotacaoNaoSelecionada()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada();
            _geradorDeEmail.GerarEmail(processoDeCotacao);
            _emailServiceMock.Verify(x => x.Enviar(It.IsAny<string>(), It.IsAny<MensagemDeEmail>()), Times.Once());
            _geradorDeMensagemDeEmailMock.Verify(x => x.FornecedoresNaoSelecionadosNoProcessoDeCotacao(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            
        }

        [TestMethod]
        public void QuandoFecharProcessoDeCotacaoQuePossuaFornecedorDaMercadoriaDeveSerEnviadoEmailParaOFornecedor()
        {
            var geradorDeEmailDeFrete = new GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete(_geradorDeMensagemDeEmailMock.Object, _emailServiceMock.Object);
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            geradorDeEmailDeFrete.GerarEmail(processoDeCotacao);
            _emailServiceMock.Verify(x => x.Enviar(It.IsAny<string>(), It.IsAny<MensagemDeEmail>()), Times.Exactly(2));
            _geradorDeMensagemDeEmailMock.Verify(x => x.AutorizacaoDeTransporte(It.IsAny<ProcessoDeCotacaoDeFrete>()), Times.Once());

        }
    }
}
