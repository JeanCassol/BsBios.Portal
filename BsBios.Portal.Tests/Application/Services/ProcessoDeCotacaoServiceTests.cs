using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly ProcessoDeCotacaoAtualizarVm _atualizacaoDoProcessoDeCotacaoVm;
        private readonly IProcessoDeCotacaoService _processoDeCotacaoService;

        public ProcessoDeCotacaoServiceTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
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

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial =  DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object);
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            _processoDeCotacaoService = new ProcessoDeCotacaoService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object);
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

        }
        [TestMethod]
        public void QuandoOProcessoEAtualizadoComSucessoEFeitoCommitNaTransacao()
        {
            _processoDeCotacaoService.AtualizarProcesso(_atualizacaoDoProcessoDeCotacaoVm);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
                        
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
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
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



    }
}
