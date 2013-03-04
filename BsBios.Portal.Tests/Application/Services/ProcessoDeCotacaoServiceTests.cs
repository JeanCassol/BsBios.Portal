using System.Linq;
using System.Collections.Generic;
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
        private readonly Mock<IFornecedores> _fornecedoresMock;
        private readonly AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm _atualizacaoDosFornecedoresVm;
        private readonly ProcessoDeCotacaoService _processoDeCotacaoService;

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
            processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0001", "FORNECEDOR 0001","fornecedor0001@empresa.com.br"));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0002", "FORNECEDOR 0002","fornecedor0001@empresa.com.br"));
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object);
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()))
                             .Returns(_fornecedoresMock.Object);
            _fornecedoresMock.Setup(x => x.List())
                             .Returns(new List<Fornecedor>()
                                 {
                                     new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor0002@empresa.com.br")
                                 });

            _processoDeCotacaoService = new ProcessoDeCotacaoService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object, _fornecedoresMock.Object);
            _atualizacaoDosFornecedoresVm = new AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm()
            {
                IdProcessoCotacao = 1,
                CodigoFornecedoresSelecionados = new[] { "FORNEC0001", "FORNEC0003" }
            };
        }

        [TestMethod]
        public void QuandoAtualizoOsFornecedoresDoProcessoDeCotacaoOcorrePersistencia()
        {
            _processoDeCotacaoService.AtualizarFornecedores(_atualizacaoDosFornecedoresVm);
            _processosDeCotacaoMock.Verify( x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }

        [TestMethod]
        public void QuandoAtualizoOsFornecedoresDoProcessoDeCotacaoComSucessoOcorreCommitNaTransacao()
        {
            _processoDeCotacaoService.AtualizarFornecedores(_atualizacaoDosFornecedoresVm);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorreAlgumErroDuranteProcessoDeAtualizacaoDeFornecedoresOcorreRollbackNaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Throws(
                                       new ExcecaoDeTeste(
                                           "Ocorreu um erro ao consultar o processo de cotação de material"));
            try
            {
                _processoDeCotacaoService.AtualizarFornecedores(_atualizacaoDosFornecedoresVm);
                Assert.Fail("Deveria ter gerado excessão");
            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
            }
            
        }

        [TestMethod]
        public void AposAtualizarOsFornecedoresDoProcessoDeCotacaoAListaFicaComOsMesmosCodigosPassadosPorParametro()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                       {
                                           Assert.IsNotNull(processoDeCotacao);
                                           Assert.AreEqual(2, processoDeCotacao.FornecedoresParticipantes.Count);
                                           Assert.AreEqual(1,processoDeCotacao.FornecedoresParticipantes.Count(x => x.Fornecedor.Codigo == "FORNEC0001"));
                                           Assert.AreEqual(1,processoDeCotacao.FornecedoresParticipantes.Count(x => x.Fornecedor.Codigo == "FORNEC0003"));
                                       });

            _processoDeCotacaoService.AtualizarFornecedores(_atualizacaoDosFornecedoresVm);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            _fornecedoresMock.Verify(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()), Times.Once());



        }
    }
}
