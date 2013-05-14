using System;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain;
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
    public class AtualizadorDeCotacaoTests
    {

        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<ICondicoesDePagamento> _condicoesDePagamentoMock;
        private readonly Mock<IIncoterms> _incotermsMock;
        private readonly IAtualizadorDeCotacaoDeMaterial _atualizadorDeCotacao;
        private static readonly CondicaoDePagamento CondicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
        private static readonly Incoterm Incoterm = DefaultObjects.ObtemIncotermPadrao();
        private static readonly ProcessoDeCotacao ProcessoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao() ;
        private readonly CotacaoMaterialInformarVm _cotacaoAtualizarVm;
        private readonly CotacaoMaterialItemInformarVm _cotacaoItemAtualizarVm;
        private Incoterm _incotermRetorno;


        public AtualizadorDeCotacaoTests()
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
                                   .Returns(ProcessoDeCotacao);

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
                    IdProcessoCotacao = ProcessoDeCotacao.Id ,
                    CodigoFornecedor = ProcessoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo ,
                    CodigoCondicaoPagamento = CondicaoDePagamento.Codigo,
                    CodigoIncoterm = Incoterm.Codigo,
                    DescricaoIncoterm = "Desc Incoterm" ,
                };

            _cotacaoItemAtualizarVm = new CotacaoMaterialItemInformarVm
                {
                    IdProcessoCotacao = ProcessoDeCotacao.Id,
                    ValorLiquido = 110,
                    ValorComImpostos = 125,
                    Mva = 0,
                    QuantidadeDisponivel = 150,
                    Impostos = new CotacaoImpostosVm()
                    {
                        IcmsAliquota = 17,
                        IcmsValor = 12,
                        IcmsStAliquota = 0,
                        IcmsStValor = 0,
                        IpiAliquota = 5,
                        IpiValor = 4,
                        PisCofinsAliquota = 3
                    },
                    IdCotacao = ProcessoDeCotacao.Id,
                    IdProcessoCotacaoItem = ProcessoDeCotacao.Itens.First().Id,
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
            //_atualizadorDeCotacao.AtualizarItemDaCotacao();
            throw new NotImplementedException();
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
            throw new NotImplementedException();
            //Assert.AreEqual(109, cotacao.ValorLiquido);
            //Assert.AreEqual(125, cotacao.ValorComImpostos);
            //Assert.AreEqual(0, cotacao.Mva);
            //Imposto icms = cotacao.Impostos.Single(x => x.Tipo == Enumeradores.TipoDeImposto.Icms);
            //Assert.AreEqual(17, icms.Aliquota);
            //Assert.AreEqual(12, icms.Valor);

            
        }
            
    }
}
