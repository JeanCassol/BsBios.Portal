using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class AlteradorDeRequisicaoDeCompraTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRequisicoesDeCompra> _requisicoesDeCompraMock;

        public AlteradorDeRequisicaoDeCompraTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _requisicoesDeCompraMock = new Mock<IRequisicoesDeCompra>(MockBehavior.Strict);
            _requisicoesDeCompraMock.Setup(x => x.BuscaPeloId(It.IsAny<int>()))
                                    .Returns(DefaultObjects.ObtemRequisicaoDeCompraPadrao());

            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback(CommonGenericMocks<RequisicaoDeCompra>.DefaultSaveCallBack(_unitOfWorkMock));
        }

        [TestMethod]
        public void QuandoBloqueioRequisicaoDeCompraComSucessoOcorrePersistencia()
        {
            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object,_requisicoesDeCompraMock.Object);
            alterador.Alterar(10);
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreAlgumErroAoAlterarRequisicaoDeCompraNaoOcorrePersistencia()
        {
            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object);
            alterador.Alterar(10);
            CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoBloqueioRequisicaoDeCompraFicaBloqueada()
        {
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback((RequisicaoDeCompra req) => Assert.AreEqual(Enumeradores.StatusRequisicaoCompra.Bloqueado,req.Status ));

            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object);
            alterador.Alterar(10);
            _requisicoesDeCompraMock.Verify(x => x.Save(It.IsAny<RequisicaoDeCompra>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof (RequisicaoDeCompraComProcessoDeCotacaoBloqueioException))]
        public void NaoDevePermtirBloquearUmaRequisicaDeCompraQueJaEstejaVinculadaComUmProcessoDeCotacaoDeMateriais()
        {
            
        }
    }
}
