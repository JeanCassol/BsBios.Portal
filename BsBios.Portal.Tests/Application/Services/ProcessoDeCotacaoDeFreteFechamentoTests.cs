using System.Linq;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoDeFreteFechamentoTests
    {
        [TestMethod]
        public void QuandoFechoUmProcessoDeCotacaoDeFreteDevePersistirAsOrdensDeTransporteGeradas()
        {
            Mock<IUnitOfWork> unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            var processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                .Returns(processosDeCotacaoMock.Object);

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            FornecedorParticipante participanteSelecionado = processoDeCotacao.FornecedoresSelecionados.First();
            processosDeCotacaoMock.Setup(x => x.Single())
                .Returns(processoDeCotacao);

            processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()));

            var geradorDeEmailMock = new Mock<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>(MockBehavior.Strict);
            geradorDeEmailMock.Setup(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()));

            var comunicacaoSapMock = new Mock<IComunicacaoSap>(MockBehavior.Strict);

            comunicacaoSapMock.Setup(x => x.EfetuarComunicacao(It.IsAny<ProcessoDeCotacao>()))
                .Returns(new ApiResponseMessage
                {
                    Retorno = new Retorno { Codigo = "200", Texto = "S" }
                });

            var ordensDeTransporteMock = new Mock<IOrdensDeTransporte>(MockBehavior.Strict);
            ordensDeTransporteMock.Setup(x => x.Save(It.IsAny<OrdemDeTransporte>()))
                .Callback((OrdemDeTransporte ordemDeTransporte) =>
                {
                    Assert.IsNotNull(ordemDeTransporte);
                    Assert.AreSame(processoDeCotacao, ordemDeTransporte.ProcessoDeCotacaoDeFrete);
                    Assert.AreSame(participanteSelecionado.Fornecedor, ordemDeTransporte.Fornecedor);
                });

            var servicoDeFechamento = new FechamentoDeProcessoDeCotacaoDeFreteService(unitOfWorkMock.Object,processosDeCotacaoMock.Object,
                geradorDeEmailMock.Object,comunicacaoSapMock.Object,ordensDeTransporteMock.Object);

            servicoDeFechamento.Executar(10);

            ordensDeTransporteMock.Verify(x => x.Save(It.IsAny<OrdemDeTransporte>()), Times.Once());
        }

    }
}
