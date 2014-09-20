using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroDeRequisicaoDeCompraTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IRequisicoesDeCompra> _requisicoesDeCompraMock;
        private readonly Mock<IUsuarios> _usuariosMock;
        private readonly Mock<IFornecedores> _fornecedoresMock;
        private readonly Mock<IProdutos> _produtosMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<IUnidadesDeMedida> _unidadesDeMedidaMock;
        private readonly ICadastroRequisicaoCompra _cadastroRequisicao;
        private static RequisicaoDeCompraVm _requisicaoDeCompraVm;

        public CadastroDeRequisicaoDeCompraTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _requisicoesDeCompraMock = new Mock<IRequisicoesDeCompra>(MockBehavior.Strict);
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                .Callback((RequisicaoDeCompra requisicaoDeCompra) => Assert.IsNotNull(requisicaoDeCompra)
                );

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);
            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>())).Returns((string login) =>
                {
                    if (login == "criador")
                    {
                        return new Usuario("Usuário Criador", "criador", null);
                    }
                    return null;
                });

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                             .Returns(new Fornecedor("FORNEC0001", "FORNECEDOR 0001", null, "", "", "",false, "endereço 0001"));

            _produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            _produtosMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                         .Returns(new Produto("PROD0001", "PRODUTO 0001", "01"));

            _unidadesDeMedidaMock = new Mock<IUnidadesDeMedida>(MockBehavior.Strict);
            _unidadesDeMedidaMock.Setup(x => x.BuscaPeloCodigoInterno(It.IsAny<string>()))
                                 .Returns(_unidadesDeMedidaMock.Object);

            _unidadesDeMedidaMock.Setup(x => x.Single())
                                 .Returns(new UnidadeDeMedida("I01","E01", "UNIDADE 01"));

            _processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()));

            _cadastroRequisicao = new CadastroRequisicaoCompra(_unitOfWorkMock.Object, _requisicoesDeCompraMock.Object,
                _usuariosMock.Object,_fornecedoresMock.Object, _produtosMock.Object, _processosDeCotacaoMock.Object,_unidadesDeMedidaMock.Object);

        }

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            _requisicaoDeCompraVm = new RequisicaoDeCompraVm()
                {
                    NumeroRequisicao = "REQ001",
                    NumeroItem =  "0001",
                    Centro =  "C001",
                    Criador =  "criador",
                    DataDeSolicitacao = DateTime.Today.AddDays(-2).ToShortDateString()  ,
                    DataDeLiberacao =  DateTime.Today.AddDays(-1).ToShortDateString(),
                    DataDeRemessa = DateTime.Today.ToShortDateString(),
                    Descricao = "Requisição de compra enviada pelo SAP" ,
                    FornecedorPretendido = "FORNEC0001" ,
                    Material = "PROD0001" ,
                    Quantidade = 100 ,
                    Requisitante = "requisitante" ,
                    UnidadeMedida = "UND"
                };
        }

        [TestMethod]
        public void QuandoCadastroUmaRequisicaoDeCommpraComSucessoERealizadoCommitNaTransacao()
        {
            _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorreErroAoCadastrarUmaRequisicaoDeCompraERealizadoRollbackNaTransacao()
        {
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Throws(new ExcecaoDeTeste("Ocorreu erro ao salvar a requisição"));
            try
            {
                _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
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
        public void QuandoCadastroUmaRequisicaoERealizadaPersistenciaDosObjetos()
        {
            _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
            _requisicoesDeCompraMock.Verify(x => x.Save(It.IsAny<RequisicaoDeCompra>()), Times.Once());
            _requisicoesDeCompraMock.Verify(x => x.Save(It.IsAny<RequisicaoDeCompra>()), Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmaRequisicaoDeCompraERealizadoConsultaNosRepositorios()
        {
            _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
            _usuariosMock.Verify(x => x.BuscaPorLogin(It.IsAny<string>()), Times.Once());            
            _fornecedoresMock.Verify(x => x.BuscaPeloCodigo(It.IsAny<string>()), Times.Once());
            _produtosMock.Verify(x => x.BuscaPeloCodigo(It.IsAny<string>()), Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmaRequisicaoEGeradaUmaNovaRequisicaoComAsPropriedadesPassadasPorParametro()
        {
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback((RequisicaoDeCompra requisicaoDeCompra) =>
                                        {
                                            Assert.IsNotNull(requisicaoDeCompra);
                                            Assert.AreEqual("criador", requisicaoDeCompra.Criador.Login);
                                            Assert.AreEqual("requisitante", requisicaoDeCompra.Requisitante);
                                            Assert.AreEqual("FORNEC0001", requisicaoDeCompra.FornecedorPretendido.Codigo);
                                            Assert.AreEqual("PROD0001", requisicaoDeCompra.Material.Codigo);
                                            Assert.AreEqual(Convert.ToDateTime(_requisicaoDeCompraVm.DataDeRemessa), requisicaoDeCompra.DataDeRemessa);
                                            Assert.AreEqual(Convert.ToDateTime(_requisicaoDeCompraVm.DataDeLiberacao), requisicaoDeCompra.DataDeLiberacao);
                                            Assert.AreEqual(Convert.ToDateTime(_requisicaoDeCompraVm.DataDeSolicitacao), requisicaoDeCompra.DataDeSolicitacao);
                                            Assert.AreEqual("C001", requisicaoDeCompra.Centro);
                                            Assert.AreEqual("I01", requisicaoDeCompra.UnidadeMedida.CodigoInterno);
                                            Assert.AreEqual(100, requisicaoDeCompra.Quantidade);
                                            Assert.AreEqual("Requisição de compra enviada pelo SAP", requisicaoDeCompra.Descricao);
                                            Assert.AreEqual("REQ001", requisicaoDeCompra.Numero);
                                            Assert.AreEqual("0001", requisicaoDeCompra.NumeroItem);
                                        });

            _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
        }

        [TestMethod]
        public void QuandoCadastroUmaRequisicaoGeraUmaProcessoDeCompraDeMaterialRelacionadoARequisicao()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                                   .Callback((ProcessoDeCotacao processoDeCotacao) =>
                                       {
                                           Assert.IsNotNull(processoDeCotacao);
                                           var processoDeCotacaoDeMaterial = (ProcessoDeCotacaoDeMaterial) processoDeCotacao;
                                           Assert.AreEqual("REQ001", processoDeCotacaoDeMaterial.RequisicaoDeCompra.Numero);
                                           Assert.AreEqual("0001", processoDeCotacaoDeMaterial.RequisicaoDeCompra.NumeroItem);
                                           Assert.AreEqual("PROD0001", processoDeCotacaoDeMaterial.Produto.Codigo);
                                           Assert.AreEqual(100, processoDeCotacaoDeMaterial.Quantidade);
                                       });
            _cadastroRequisicao.NovaRequisicao(_requisicaoDeCompraVm);
        }
    }
}
