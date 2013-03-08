using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain;
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
    public class AtualizadorDeCotacaoTests
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<ICondicoesDePagamento> _condicoesDePagamentoMock;
        private readonly Mock<IIncoterms> _incotermsMock;
        private readonly IAtualizadorDeCotacao _atualizadorDeCotacao;
        private static readonly CondicaoDePagamento CondicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
        private static readonly Incoterm Incoterm = DefaultObjects.ObtemIncotermPadrao();
        private static readonly ProcessoDeCotacao _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao() ;
        private readonly CotacaoInformarVm _cotacaoAtualizarVm;
        private Incoterm _incotermRetorno;


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

            //ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            //processoDeCotacaoDeMaterial.Atualizar(DateTime.Today);
            //processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br"));
            //processoDeCotacaoDeMaterial.AdicionarFornecedor(new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0001@empresa.com.br"));
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object)
                                   .Callback((int id) =>
                                       {
                                           _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                                           _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                                       });
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(_processoDeCotacao);

            _incotermsMock = new Mock<IIncoterms>(MockBehavior.Strict);
            _incotermsMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                .Returns(_incotermsMock.Object)
                          .Callback((string codigo) =>
                              {
                                  _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                                  _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                                  _incotermRetorno = Incoterm.Codigo == codigo ? Incoterm : null;
                              });
            _incotermsMock.Setup(x => x.Single())
                          .Returns(() => _incotermRetorno);

            _condicoesDePagamentoMock = new Mock<ICondicoesDePagamento>(MockBehavior.Strict);
            _condicoesDePagamentoMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                .Returns((string codigo) => codigo == CondicaoDePagamento.Codigo ? CondicaoDePagamento : null)
                          .Callback((string codigo) =>
                              {
                                  _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                                  _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                              });

            _atualizadorDeCotacao = new AtualizadorDeCotacao(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object,_incotermsMock.Object, _condicoesDePagamentoMock.Object);

            
            _cotacaoAtualizarVm = new CotacaoInformarVm()
                {
                    IdProcessoCotacao = _processoDeCotacao.Id ,
                    CodigoFornecedor = _processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo ,
                    CodigoCondicaoPagamento = CondicaoDePagamento.Codigo,
                    CodigoIncoterm = Incoterm.Codigo,
                    DescricaoIncoterm = "Desc Incoterm" ,
                    PossuiImpostos = true,
                    ValorLiquido = 110 ,
                    ValorComImpostos =  125,
                    Mva = 0 ,
                    IcmsAliquota = 17 ,
                    IcmsValor = 12
                };
        }

        [TestMethod]
        public void QuandoAtualizarCotacaoDoFornecedorOcorrePersistencia()
        {
            _atualizadorDeCotacao.Atualizar(_cotacaoAtualizarVm);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
        }

        [TestMethod]
        public void QuandoAtualizaCotacaoDoFornecedorComSucessoOcorreCommitNaTransacao()
        {
            _atualizadorDeCotacao.Atualizar(_cotacaoAtualizarVm);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorrerAlgumErroAoAtualizarCotacaoDoFornecedorOcorreRollbackNaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()) )
                .Throws(new ExcecaoDeTeste("Erro ao consultar Processo de Cotação"));
            try
            {
                _atualizadorDeCotacao.Atualizar(_cotacaoAtualizarVm);
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
        public void QuandoAtualizaCotacaoDoFornecedorComSucessoAsPropriedadesDaCotacaoSaoAlteradas()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                .Callback((ProcessoDeCotacao processoDeCotacao) =>
                    {
                        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                        Assert.IsNotNull(processoDeCotacao);
                        FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresParticipantes.First();
                        Assert.IsNotNull(fornecedorParticipante.Cotacao);
                        Cotacao cotacao = fornecedorParticipante.Cotacao;
                        Assert.AreSame(CondicaoDePagamento, cotacao.CondicaoDePagamento);
                        Assert.AreSame(Incoterm, cotacao.Incoterm);
                        Assert.AreEqual("Desc Incoterm", cotacao.DescricaoIncoterm);
                        Assert.AreEqual(110, cotacao.ValorLiquido);
                        Assert.AreEqual(125, cotacao.ValorComImpostos);
                        Assert.AreEqual(0, cotacao.Mva);
                        Imposto icms = cotacao.Impostos.Single(x => x.Tipo == Enumeradores.TipoDeImposto.Icms);
                        Assert.AreEqual(17, icms.Aliquota);
                        Assert.AreEqual(12, icms.Valor);

                    });
            _atualizadorDeCotacao.Atualizar(_cotacaoAtualizarVm);
            
        }
            
    }
}
