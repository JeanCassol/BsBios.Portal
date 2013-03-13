using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroProdutoTests
    {
        private readonly ICadastroProduto _cadastroProduto;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProdutos> _produtosMock;
        private readonly ProdutoCadastroVm _produtoPadrao;
        private readonly IList<ProdutoCadastroVm> _produtosPadrao;
        private readonly IList<Produto> _produtosConsulta;
        public CadastroProdutoTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _produtosConsulta = new List<Produto>();
            _produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Callback((Produto produto) => Assert.IsNotNull(produto));
            _produtosMock.Setup(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>()))
                .Callback((string[] codigos) =>
                {
                    if (codigos.Contains("PROD0001"))
                    {
                        _produtosConsulta.Add(new ProdutoParaAtualizacao("PROD0001", "PRODUTO 0001", "01"));    
                    }
                })
                .Returns(_produtosMock.Object);

            _produtosMock.Setup(x => x.List()).Returns(_produtosConsulta);

            _cadastroProduto = new CadastroProduto(_unitOfWorkMock.Object, _produtosMock.Object);

            _produtoPadrao = new ProdutoCadastroVm()
                {
                    Codigo = "SAP 0001",
                    Descricao = "PRODUTO 0001",
                    Tipo = "01"
                };

            _produtosPadrao = new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            Codigo = "PROD0001",
                            Descricao =  "PRODUTO 0001",
                            Tipo = "01"
                        },
                    new ProdutoCadastroVm()
                        {
                            Codigo = "PROD0002" ,
                            Descricao = "PRODUTO 0002",
                            Tipo = "02"
                        }
                };
        }

        [TestMethod]
        public void QuandoCadastroUmNovoProdutoEPersistidoNoBanco()
        {
            _cadastroProduto.Novo(_produtoPadrao);
            _produtosMock.Verify(x => x.Save(It.IsAny<Produto>()), Times.Once());
            _produtosMock.Verify(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoAoCadastrarUmProdutoFazRollback()
        {
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o produto"));
            try
            {
                _cadastroProduto.Novo(_produtoPadrao);
                Assert.Fail("Deveria ter gerado excessão");
                
            }
            catch(ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoCadastroUmaListaDeProdutosTudoDeveEstarDentroDeUmaUnicaTransacao()
        {
            _cadastroProduto.AtualizarProdutos(_produtosPadrao);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoCadastrarUmaListaDeProdutosDeveSalvarTodosOsRegistros()
        {
            _cadastroProduto.AtualizarProdutos(_produtosPadrao);
            _produtosMock.Verify(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]> ()), Times.Once());
            _produtosMock.Verify(x => x.Save(It.IsAny<Produto>()),Times.Exactly(_produtosPadrao.Count));
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoAoCadastrarUmaListaDeProdutosFazRollback()
        {
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o produto"));
            try
            {
                _cadastroProduto.Novo(_produtoPadrao);
                Assert.Fail("Deveria ter gerado excessão");

            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                //executa uma única vez porque no primeiro save já vai gerar exceção
                _produtosMock.Verify(x => x.Save(It.IsAny<Produto>()), Times.Once()); 
            }
        }

        [TestMethod]
        public void QuandoReceberUmProdutoExistenteDeveAtualizar()
        {
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Callback((Produto produto) =>
                {
                    Assert.IsNotNull(produto);
                    Assert.IsInstanceOfType(produto, typeof(ProdutoParaAtualizacao));
                    Assert.AreEqual("PROD0001", produto.Codigo);
                    Assert.AreEqual("PRODUTO 0001 Atualizado",produto.Descricao);
                    Assert.AreEqual("03", produto.Tipo);
                });

            _cadastroProduto.AtualizarProdutos(new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            Codigo = "PROD0001"  ,
                            Descricao = "PRODUTO 0001 Atualizado",
                            Tipo = "03"
                        }
                });

        }

        [TestMethod]
        public void QuandoReceberUmProdutoNovoDeveAdicionar()
        {

            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Callback((Produto produto) =>
            {
                Assert.IsNotNull(produto);
                Assert.IsNotInstanceOfType(produto, typeof(ProdutoParaAtualizacao));
                Assert.AreEqual("PROD0002", produto.Codigo);
                Assert.AreEqual("PRODUTO 0002", produto.Descricao);
                Assert.AreEqual("02", produto.Tipo);
            });

            _cadastroProduto.AtualizarProdutos(new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            Codigo = "PROD0002",
                            Descricao = "PRODUTO 0002",
                            Tipo = "02"
                        }
                });
        }
    }

    public class ProdutoParaAtualizacao: Produto
    {
        public ProdutoParaAtualizacao(string codigo, string descricao, string tipo) : base(codigo, descricao, tipo)
        {
        }
    }
}
