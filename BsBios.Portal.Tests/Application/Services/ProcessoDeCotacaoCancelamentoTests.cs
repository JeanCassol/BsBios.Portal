using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoCancelamentoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly ICancelamentoDeProcessoDeCotacaoService _cancelamentoDeProcessoDeCotacaoService;
        private readonly ProcessoDeCotacaoDeFrete _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComFornecedor();

        public ProcessoDeCotacaoCancelamentoTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback(
                                       (ProcessoDeCotacao processoDeCotacao) =>
                                       {
                                           _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                                           _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                                           Assert.IsNotNull(processoDeCotacao);
                                           foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
                                           {
                                               Assert.IsNotNull(fornecedorParticipante);
                                           }
                                       });

            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                .Returns(_processosDeCotacaoMock.Object);

            _processosDeCotacaoMock.Setup(x => x.Single()).Returns(() => _processoDeCotacao);

            _cancelamentoDeProcessoDeCotacaoService = new CancelamentoDeProcessoDeCotacaoService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object);
        }

        [TestMethod]
        public void QuandoOProcessoECanceladoOcorrePersistencia()
        {
            _cancelamentoDeProcessoDeCotacaoService.Executar(20);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreErroAoFecharProcessoOcorreRollbackDaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Throws(new ExcecaoDeTeste("Erro ao consultar Processo."));
            try
            {
                _cancelamentoDeProcessoDeCotacaoService.Executar(20);
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoOProcessoEFechadoComSucessoAsPropriedadesDoProcessoFicamCorretas()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                .Callback((ProcessoDeCotacao processoDeCotacao) =>
                {
                    Assert.IsNotNull(processoDeCotacao);
                    Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Cancelado,
                        processoDeCotacao.Status);
                });


            _cancelamentoDeProcessoDeCotacaoService.Executar(20);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }
    }
}
