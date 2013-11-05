using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
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
    public class ProcessoDeCotacaoDeMaterialFechamentoTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao> _geradorDeEmailMock;
        private readonly Mock<IComunicacaoSap> _comunicacaoSapMock;
        private readonly IFechamentoDeProcessoDeCotacaoService _fechamentoDeProcessoDeCotacaoService;
        private ProcessoDeCotacaoDeMaterial _processoDeCotacao;


        public ProcessoDeCotacaoDeMaterialFechamentoTests()
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
                        if (idProcessoCotacao == 30)
                        {
                            _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
                        }
                    });

            _processosDeCotacaoMock.Setup(x => x.Single()).Returns(() => _processoDeCotacao);

            _geradorDeEmailMock = new Mock<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>(MockBehavior.Strict);
            _geradorDeEmailMock.Setup(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()));

            _comunicacaoSapMock = new Mock<IComunicacaoSap>(MockBehavior.Strict);
            _comunicacaoSapMock.Setup(x => x.EfetuarComunicacao(It.IsAny<ProcessoDeCotacao>()))
                .Returns(new ApiResponseMessage
                    {
                        Retorno = new Retorno{Codigo = "200", Texto = "S"}
                    });

            _fechamentoDeProcessoDeCotacaoService = new FechamentoDeProcessoDeCotacaoDeMaterialService(_unitOfWorkMock.Object,_processosDeCotacaoMock.Object,
                _geradorDeEmailMock.Object,_comunicacaoSapMock.Object);

        }

 
        #region Testes de Fechamento do Processo
        [TestMethod]
        public void QuandoOProcessoEFechadoOcorrePersistencia()
        {
            _fechamentoDeProcessoDeCotacaoService.Executar(20);
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
                _fechamentoDeProcessoDeCotacaoService.Executar(20);
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
                                       Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Fechado,
                                                       processoDeCotacao.Status);
                                   });


            _fechamentoDeProcessoDeCotacaoService.Executar(20);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOProcessoEFechadoComSucessoEEnviadoEmailParaOsFornecedoresSelecionados()
        {
            _fechamentoDeProcessoDeCotacaoService.Executar(20);
            _geradorDeEmailMock.Verify(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()), Times.Once());            
        }

        [TestMethod]
        public void QuandoTentoFecharUmProcessoDeCotacaoJaFechadoNaoEnviaEmailNemSeComunicaComSap()
        {
            try
            {
                _fechamentoDeProcessoDeCotacaoService.Executar(30);
                Assert.Fail("Deveria ter gerado excessão");
            }
            catch (FecharProcessoDeCotacaoFechadoException)
            {
                _comunicacaoSapMock.Verify(x => x.EfetuarComunicacao(It.IsAny<ProcessoDeCotacao>()), Times.Never());
                _geradorDeEmailMock.Verify(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()), Times.Never());
            }

        }

        #endregion
    }
}
