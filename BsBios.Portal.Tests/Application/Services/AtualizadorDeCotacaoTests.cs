using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
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
    public class AtualizadorDeCotacaoTests
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<ICondicoesDePagamento> _condicoesDePagamentoMock;
        private readonly Mock<IIncoterms> _incotermsMock;
        private readonly IAtualizadorDeCotacaoDeMaterial _atualizadorDeCotacao;
        private static readonly CondicaoDePagamento CondicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
        private static readonly Incoterm Incoterm = DefaultObjects.ObtemIncotermPadrao();
        private readonly ProcessoDeCotacao _processoDeCotacao ;
        private readonly CotacaoMaterialInformarVm _cotacaoAtualizarVm;
        private readonly CotacaoMaterialItemInformarVm _cotacaoItemAtualizarVm;
        private Incoterm _incotermRetorno;

        public AtualizadorDeCotacaoTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
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

            _atualizadorDeCotacao = new AtualizadorDeCotacaoDeMaterial(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object,_incotermsMock.Object, _condicoesDePagamentoMock.Object);

            
            _cotacaoAtualizarVm = new CotacaoMaterialInformarVm()
                {
                    IdProcessoCotacao = _processoDeCotacao.Id ,
                    CodigoFornecedor = _processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo ,
                    CodigoCondicaoPagamento = CondicaoDePagamento.Codigo,
                    CodigoIncoterm = Incoterm.Codigo,
                    DescricaoIncoterm = "Desc Incoterm" ,
                };

            _cotacaoItemAtualizarVm = new CotacaoMaterialItemInformarVm
                {
                    IdProcessoCotacao = _processoDeCotacao.Id,
                    IdCotacao = 0,
                    IdProcessoCotacaoItem = _processoDeCotacao.Itens.First().Id,
                    Preco = 110,
                    Mva = 0,
                    QuantidadeDisponivel = 150,
                    Impostos = new CotacaoImpostosVm
                    {
                        IcmsAliquota = 17,
                        IcmsValor = 12,
                        IcmsStAliquota = 0,
                        IcmsStValor = 0,
                        IpiAliquota = 5,
                        IpiValor = 4,
                        PisCofinsAliquota = 3
                    },
                    ObservacoesDoFornecedor = "observações do fornecedor" ,
                    PrazoDeEntrega = DateTime.Today.AddDays(15).ToShortDateString()
                    
                };
        }

        [TestMethod]
        public void QuandoAtualizarCotacaoDoFornecedorOcorrePersistencia()
        {
            _atualizadorDeCotacao.AtualizarCotacao(_cotacaoAtualizarVm);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoAtualizoItemDaCotacaoDoFornecedorOcorrePersistencia()
        {
            _atualizadorDeCotacao.AtualizarItemDaCotacao(_cotacaoItemAtualizarVm);
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorrerAlgumErroAoAtualizarCotacaoDoFornecedorOcorreRollbackNaTransacao()
        {
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()) )
                .Throws(new ExcecaoDeTeste("Erro ao consultar Processo de Cotação"));
            try
            {
                _atualizadorDeCotacao.AtualizarCotacao(_cotacaoAtualizarVm);
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
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
                        var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao;
                        Assert.AreSame(CondicaoDePagamento, cotacao.CondicaoDePagamento);
                        Assert.AreSame(Incoterm, cotacao.Incoterm);
                        Assert.AreEqual("Desc Incoterm", cotacao.DescricaoIncoterm);

                    });
            _atualizadorDeCotacao.AtualizarCotacao(_cotacaoAtualizarVm);
            
        }

        [TestMethod]
        public void QuandoAtualizaItemDaCotacaoDoFornecedorComSucessoAsPropriedadesSaoAlteradas()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                .Callback((ProcessoDeCotacao processoDeCotacao) =>
                {
                    _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                    _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                    Assert.IsNotNull(processoDeCotacao);
                    FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresParticipantes.First();
                    Assert.IsNotNull(fornecedorParticipante.Cotacao);
                    var cotacaoItem = (CotacaoMaterialItem) fornecedorParticipante.Cotacao.Itens.First();

                    Assert.AreEqual(110, cotacaoItem.Preco);
                    Assert.AreEqual((decimal) 115.5, cotacaoItem.ValorComImpostos);
                    Assert.AreEqual(0, cotacaoItem.Mva);
                    Imposto icms = cotacaoItem.Impostos.Single(x => x.Tipo == Enumeradores.TipoDeImposto.Icms);
                    Assert.AreEqual(17, icms.Aliquota);
                    Assert.AreEqual((decimal) 18.7, icms.Valor);
                });
            _atualizadorDeCotacao.AtualizarItemDaCotacao(_cotacaoItemAtualizarVm);

        }
            
    }
}
