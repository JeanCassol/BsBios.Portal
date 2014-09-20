using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoDeMaterialServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly ProcessoDeCotacaoAtualizarVm _atualizacaoDoProcessoDeCotacaoVm;
        private readonly IProcessoDeCotacaoDeMaterialService _processoDeCotacaoService;

        public ProcessoDeCotacaoDeMaterialServiceTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback(
                                       (ProcessoDeCotacao processoDeCotacao) =>
                                           {
                                               Assert.IsNotNull(processoDeCotacao);
                                               foreach (var fornecedorParticipante in processoDeCotacao.FornecedoresParticipantes)
                                               {
                                                   Assert.IsNotNull(fornecedorParticipante);
                                               }
                                           });

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object);
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            _processoDeCotacaoService = new ProcessoDeCotacaoDeMaterialService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object);
            _atualizacaoDoProcessoDeCotacaoVm = new ProcessoDeCotacaoAtualizarVm()
                {
                    Id = 1,
                    DataLimiteRetorno = DateTime.Today.AddDays(10)
                };
        }

        #region Testes de atualização do processo
        [TestMethod]
        public void QuandoAtualizaProcessoOcorrePersistencia()
        {
            _processoDeCotacaoService.AtualizarProcesso(_atualizacaoDoProcessoDeCotacaoVm);

            _processosDeCotacaoMock.Verify(x  => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);

        }

        [TestMethod]
        public void QuandoOcorreAlgumErroAoAtualizarProcessoEFeitoRollbackNaTransacao()
        {
            try
            {
                _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                        .Throws(new ExcecaoDeTeste("Ocorreu um erro ao consultar o Processo de Cotação."));
                _processoDeCotacaoService.AtualizarProcesso(_atualizacaoDoProcessoDeCotacaoVm);
                Assert.Fail("Deveria ter gerado exceção");

            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
            
        }
        [TestMethod]
        public void QuandoOProcessoEAtualizadoComSucessoAsPropriedadesDoProcessoSaoAtualizadasCorretamente()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                       {
                                           Assert.IsNotNull(processoDeCotacao);
                                           Assert.AreEqual(DateTime.Today.AddDays(10),
                                                           processoDeCotacao.DataLimiteDeRetorno);

                                       });
            _processoDeCotacaoService.AtualizarProcesso(_atualizacaoDoProcessoDeCotacaoVm);
            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }

        #endregion

        #region Testes de Verificação da quantidade adquirida
        [TestMethod]
        public void ServicoDeVerificacaoDeQuantidadeAdquiridaRetornaResultadoDaComparacao()
        {
            VerificacaoDeQuantidadeAdquiridaVm verificacaoVm = _processoDeCotacaoService.VerificarQuantidadeAdquirida(10, 1001);
            Assert.AreEqual(1000, verificacaoVm.QuantidadeSolicitadaNoProcessoDeCotacao);
            Assert.IsTrue(verificacaoVm.SuperouQuantidadeSolicitada);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
        }

        #endregion



    }
}
