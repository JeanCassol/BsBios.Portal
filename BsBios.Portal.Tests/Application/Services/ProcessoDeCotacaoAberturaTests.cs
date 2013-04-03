using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
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
    public class ProcessoDeCotacaoAberturaTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<IGeradorDeEmailDeAberturaDeProcessoDeCotacao> _geradorDeEmailMock;
        private readonly Mock<IComunicacaoSap> _comunicacaoSapMock;
        private readonly Mock<IGerenciadorUsuario> _gerenciadorUsuarioMock;
        private ProcessoDeCotacaoDeMaterial _processoDeCotacao;
        private readonly IAberturaDeProcessoDeCotacaoService _service;


        public ProcessoDeCotacaoAberturaTests()
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
                        var cotacao = _processoDeCotacao.InformarCotacao(codigoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                           DefaultObjects.ObtemIncotermPadrao(), "inc", 150, null, 100, DateTime.Today.AddMonths(1), "obs fornec");
                        _processoDeCotacao.SelecionarCotacao(cotacao.Id, 100, DefaultObjects.ObtemIvaPadrao());
                    }
                });

            _processosDeCotacaoMock.Setup(x => x.Single()).Returns(() => _processoDeCotacao);

            _comunicacaoSapMock = new Mock<IComunicacaoSap>(MockBehavior.Strict);
            _comunicacaoSapMock.Setup(x => x.EfetuarComunicacao(It.IsAny<ProcessoDeCotacao>()))
                .Returns(new ApiResponseMessage
                    {
                        Retorno = new Retorno
                            {
                                Codigo = "200",
                                Texto = "S"
                            }
                    });

            _geradorDeEmailMock = new Mock<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(MockBehavior.Strict);
            _geradorDeEmailMock.Setup(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()));

            _gerenciadorUsuarioMock = new Mock<IGerenciadorUsuario>(MockBehavior.Strict);
            _gerenciadorUsuarioMock.Setup(x => x.CriarSenhaParaUsuariosSemSenha(It.IsAny<string[]>()));

            _service = new AberturaDeProcessoDeCotacaoDeFrete(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object,
                _geradorDeEmailMock.Object,_comunicacaoSapMock.Object,_gerenciadorUsuarioMock.Object);
        }

        #region Testes de Abertura do Processo
        [TestMethod]
        public void QuandoOProcessoEAbertoOcorrePersistencia()
        {
            _service.Executar(10);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }

        [TestMethod]
        public void QuandoOProcessoEAbertoComSucessoOcorreCommitDaTransacao()
        {
            _service.Executar(10);
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
                _service.Executar(10);
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

            _service.Executar(10);

            _processosDeCotacaoMock.Verify(x => x.BuscaPorId(It.IsAny<int>()), Times.Once());
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());

        }
        [TestMethod]
        public void QuandoOProcessoEAbertoComSucessoEEnviadoEmailParaOsFornecedores()
        {
            _service.Executar(20);
            _geradorDeEmailMock.Verify(x => x.GerarEmail(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }

        [TestMethod]
        public void QuandoProcessoEAbertoEnviaEmailComNovaSenhaParaFornecedoresQueNaoPossuemSenha()
        {
            _service.Executar(10);

            _gerenciadorUsuarioMock.Verify( x => x.CriarSenhaParaUsuariosSemSenha(It.IsAny<string[]>()), Times.Once());
        }
        #endregion
    }
}
