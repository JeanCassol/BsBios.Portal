using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class AtualizadorDeCotacaoTests
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly IAtualizadorDeCotacao _atualizadorDeCotacao;
        private ProcessoDeCotacao _processoDeCotacao;

        public AtualizadorDeCotacaoTests()
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

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br"));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0001@empresa.com.br"));
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object)
                                   .Callback((int id) =>
                                       {
                                           _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                                           _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                                       });
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            _atualizadorDeCotacao = new AtualizadorDeCotacao();
        }

        [TestMethod]
        public void QuandoAtualizarCotacaoDoFornecedorOcorrePersistencia()
        {
        }

        [TestMethod]
        public void QuandoAtualizaCotacaoDoFornecedorComSucessoOcorreCommitNaTransacao()
        {
            
        }

        [TestMethod]
        public void QuandoOcorrerAlgumErroAoAtualizarCotacaoDoFornecedorOcorreRollbackNaTransacao()
        {
            
        }

        [TestMethod]
        public void QuandoAtualizaCotacaoDoFornecedorComSucessoAsPropriedadesDaCotacaoSaoAlteradas()
        {
            
        }
            
    }
}
