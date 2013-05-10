using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
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

            //ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            var processoDeCotacaoDeMaterial = new ProcessoDeCotacaoDeMaterialParaAtualizacao();
            //processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            //processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object);
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            _processoDeCotacaoService = new ProcessoDeCotacaoDeMaterialService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object);
            _atualizacaoDoProcessoDeCotacaoVm = new ProcessoDeCotacaoAtualizarVm()
                {
                    Id = 1,
                    DataLimiteRetorno = DateTime.Today.AddDays(10),
                    Requisitos = "requisitos do processo"
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
        public void QuandoCriaUmNovoProcessoAsPropriedadesSaoCriadasCorretamente()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                   {
                                       Assert.IsNotNull(processoDeCotacao);
                                       Assert.IsNotInstanceOfType(processoDeCotacao, typeof(ProcessoDeCotacaoDeMaterialParaAtualizacao));
                                       Assert.AreEqual(DateTime.Today.AddDays(10), processoDeCotacao.DataLimiteDeRetorno);
                                       Assert.AreEqual("requisitos de criação", processoDeCotacao.Requisitos);
                                   });
            _processoDeCotacaoService.AtualizarProcesso(new ProcessoDeCotacaoAtualizarVm
                {
                    DataLimiteRetorno = DateTime.Today.AddDays(10),
                    Requisitos = "requisitos de criação"
                });
            
        }
        [TestMethod]
        public void QuandoOProcessoEAtualizadoComSucessoAsPropriedadesDoProcessoSaoAtualizadasCorretamente()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                       {
                                           Assert.IsNotNull(processoDeCotacao);
                                           Assert.IsInstanceOfType(processoDeCotacao, typeof(ProcessoDeCotacaoDeMaterialParaAtualizacao));
                                           Assert.AreEqual(DateTime.Today.AddDays(10),processoDeCotacao.DataLimiteDeRetorno);
                                           Assert.AreEqual("requisitos do processo", processoDeCotacao.Requisitos);
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
            var processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());

            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(processoDeCotacaoDeMaterial);

            VerificacaoDeQuantidadeAdquiridaVm verificacaoVm = _processoDeCotacaoService.VerificarQuantidadeAdquirida(10,0, 1001);
            Assert.AreEqual(1000, verificacaoVm.QuantidadeSolicitadaNoProcessoDeCotacao);
            Assert.IsTrue(verificacaoVm.SuperouQuantidadeSolicitada);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
        }

        #endregion

    }

    internal class ProcessoDeCotacaoDeMaterialParaAtualizacao: ProcessoDeCotacaoDeMaterial
    {
    }
}
