using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoStatusServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly IProcessoDeCotacaoStatusService _processoDeCotacaoStatusService;
        private ProcessoDeCotacaoDeMaterial _processoDeCotacao;


        public ProcessoDeCotacaoStatusServiceTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
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
                .Returns(_processosDeCotacaoMock.Object)
                .Callback((int idProcessoCotacao) =>
                    {
                        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                        if (idProcessoCotacao == 10)
                        {
                            _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
                            _processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
                        }
                        if (idProcessoCotacao == 20)
                        {
                            _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
                            var codigoFornecedor = _processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
                            var cotacao =  _processoDeCotacao.InformarCotacao(codigoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                               DefaultObjects.ObtemIncotermPadrao(), "inc", 150, null, 100, DateTime.Today.AddMonths(1), "obs fornec");
                            _processoDeCotacao.SelecionarCotacao(cotacao.Id, 100, DefaultObjects.ObtemIvaPadrao());
                        }
                    });

            _processosDeCotacaoMock.Setup(x => x.Single()).Returns(() => _processoDeCotacao);

            _processoDeCotacaoStatusService = new ProcessoDeCotacaoStatusService(_unitOfWorkMock.Object,_processosDeCotacaoMock.Object);

        }

        #region Testes de Abertura do Processo
        [TestMethod]
        public void QuandoOProcessoEAbertoOcorrePersistencia()
        {
            _processoDeCotacaoStatusService.AbrirProcesso(10);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOProcessoEAbertoComSucessoOcorreCommitDaTransacao()
        {
            _processoDeCotacaoStatusService.AbrirProcesso(10);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());

        }

        [TestMethod]
        public void QuandoOcorreErroAoAbrirProcessoOcorreRollbackDaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Throws(new ExcecaoDeTeste("Erro ao consultar Processo."));
            try
            {
                _processoDeCotacaoStatusService.AbrirProcesso(10);
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
            }
        }

        [TestMethod]
        public void QuandoOProcessoEAbertoComSucessoAsPropriedadesDoProcessoFicamCorretas()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                   {
                                       Assert.IsNotNull(processoDeCotacao);
                                       Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto,
                                                       processoDeCotacao.Status);
                                   });

            _processoDeCotacaoStatusService.AbrirProcesso(10);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOProcessoEAbertoComSucessoEEnviadoEmailParaOsFornecedores()
        {
            Assert.Fail("Não implementado");
        }
        #endregion



        #region Testes de Fechamento do Processo
        [TestMethod]
        public void QuandoOProcessoEFechadoOcorrePersistencia()
        {
            _processoDeCotacaoStatusService.FecharProcesso(20);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOProcessoEFechadoComSucessoOcorreCommitDaTransacao()
        {
            _processoDeCotacaoStatusService.FecharProcesso(20);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());

        }

        [TestMethod]
        public void QuandoOcorreErroAoFecharProcessoOcorreRollbackDaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Throws(new ExcecaoDeTeste("Erro ao consultar Processo."));
            try
            {
                _processoDeCotacaoStatusService.FecharProcesso(20);
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
            }
        }

        [TestMethod]
        public void QuandoOProcessoEFechadoComSucessoAsPropriedadesDoProcessoFicamCorretas()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                   {
                                       Assert.IsNotNull(processoDeCotacao);
                                       Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Fechado,
                                                       processoDeCotacao.Status);
                                   });

            _processoDeCotacaoStatusService.FecharProcesso(20);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOProcessoEFechadoComSucessoEEnviadoEmailParaOsFornecedores()
        {
            Assert.Fail("Não implementado");
        }
        #endregion
    }
}
