using System;
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
        private readonly Mock<IProcessosDeCotacaoDeMaterial> _processosDeCotacaoDeMaterialMock;

        public AlteradorDeRequisicaoDeCompraTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _requisicoesDeCompraMock = new Mock<IRequisicoesDeCompra>(MockBehavior.Strict);
            _requisicoesDeCompraMock.Setup(x => x.BuscaPeloId(It.IsAny<int>()))
                                    .Returns(DefaultObjects.ObtemRequisicaoDeCompraPadrao());

            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback(CommonGenericMocks<RequisicaoDeCompra>.DefaultSaveCallBack(_unitOfWorkMock));

            _processosDeCotacaoDeMaterialMock = new Mock<IProcessosDeCotacaoDeMaterial>(MockBehavior.Strict);
            _processosDeCotacaoDeMaterialMock.Setup(x => x.GeradosPelaRequisicaoDeCompra(It.IsAny<int>()))
                                             .Returns(_processosDeCotacaoDeMaterialMock.Object);

            _processosDeCotacaoDeMaterialMock.Setup(x => x.Count())
                                             .Returns(0);
        }

        [TestMethod]
        public void QuandoBloqueioRequisicaoDeCompraComSucessoOcorrePersistencia()
        {
            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object,_requisicoesDeCompraMock.Object, _processosDeCotacaoDeMaterialMock.Object);
            alterador.Bloquear(10);
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreAlgumErroAoBloquearRequisicaoDeCompraNaoOcorrePersistencia()
        {
            //faço disparar uma exceção no método save do repositório
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Throws(new ExcecaoDeTeste("erro ao salvar requisição"));
            try
            {
                var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object, _processosDeCotacaoDeMaterialMock.Object);
                alterador.Bloquear(10);
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (Exception)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoBloqueioRequisicaoDeCompraFicaBloqueada()
        {
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback((RequisicaoDeCompra req) => Assert.AreEqual(Enumeradores.StatusRequisicaoCompra.Bloqueado,req.Status ));

            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object, _processosDeCotacaoDeMaterialMock.Object);
            alterador.Bloquear(10);
            _requisicoesDeCompraMock.Verify(x => x.Save(It.IsAny<RequisicaoDeCompra>()), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof (RequisicaoDeCompraComProcessoDeCotacaoBloqueioException))]
        public void NaoDevePermtirBloquearUmaRequisicaDeCompraQueJaEstejaVinculadaComUmProcessoDeCotacaoDeMateriais()
        {
            //configuro count para retornar 1. Com isso consulta que verifica o número de processo vinculados a requisicaodecompra retorna 1.
            _processosDeCotacaoDeMaterialMock.Setup(x => x.Count()).Returns(1);
            var alterador = new AlteradorDeRequisicaoDeCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object, _processosDeCotacaoDeMaterialMock.Object);
            alterador.Bloquear(10);
            _processosDeCotacaoDeMaterialMock.Verify(x => x.Count(), Times.Once());
            CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
        }
    }
}
