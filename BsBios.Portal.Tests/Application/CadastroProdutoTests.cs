using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application
{
    [TestClass]
    public class CadastroProdutoTests
    {
        private readonly ICadastroProduto _cadastroProduto;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProdutos> _produtosMock;
        private readonly ProdutoCadastroVm _produtoPadrao;
        private readonly IList<ProdutoCadastroVm> _produtosPadrao;
        //private readonly Mock<Produto> _produtoMock;
        private readonly Mock<ICadastroProdutoOperacao> _atualizadorProdutoMock; 
        public CadastroProdutoTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            //_produtoMock = new Mock<Produto>(MockBehavior.Strict);
            //_produtoMock.Setup(x => x.AtualizaDescricao(It.IsAny<string>()));
            _produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Callback((Produto produto) => Assert.IsNotNull(produto));
            _produtosMock.Setup(x => x.BuscaPorCodigoSap(It.IsAny<string>())).Returns((string p) => p == "PROD0001" ? 
                new Produto("PROD0001", "PRODUTO 0001","01") : null);

            _atualizadorProdutoMock  = new Mock<ICadastroProdutoOperacao>(MockBehavior.Strict);
            _atualizadorProdutoMock.Setup(x => x.Atualizar(It.IsAny<Produto>(), It.IsAny<ProdutoCadastroVm>()));
            _atualizadorProdutoMock.Setup(x => x.Criar(It.IsAny<ProdutoCadastroVm>())).Returns(new Produto("PROD0002", "PRODUTO 0002","02"));
            _cadastroProduto = new CadastroProduto(_unitOfWorkMock.Object, _produtosMock.Object,_atualizadorProdutoMock.Object);

            _produtoPadrao = new ProdutoCadastroVm()
                {
                    CodigoSap = "SAP 0001",
                    Descricao = "PRODUTO 0001",
                    Tipo = "01"
                };

            _produtosPadrao = new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "PROD0001",
                            Descricao =  "PRODUTO 0001",
                            Tipo = "01"
                        },
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "PROD0002" ,
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
        }

        [TestMethod]
        public void QuandoCadastroUmNovoProdutoComSucessoFazCommitNaTransacao()
        {
            _cadastroProduto.Novo(_produtoPadrao);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x =>x.RollBack(), Times.Never());
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
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
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
            _produtosMock.Verify(x => x.BuscaPorCodigoSap(It.IsAny<string>()), Times.Exactly(_produtosPadrao.Count));
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

            //var cadastroProduto = new CadastroProduto(_unitOfWorkMock.Object, _produtosMock.Object);
            _cadastroProduto.AtualizarProdutos(new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "PROD0001"  ,
                            Descricao = "PRODUTO 0001 Atualizado"
                        }
                });

            _atualizadorProdutoMock.Verify(x => x.Atualizar(It.IsAny<Produto>(),It.IsAny<ProdutoCadastroVm>()),Times.Once());

            //_produtoMock.Verify(x => x.AtualizaDescricao(It.IsAny<string>()), Times.Once());

            //var produtoAtualizado = produtosAtualizados.First();
            //Assert.AreEqual("PROD0001", produtoAtualizado.Codigo);
            //Assert.AreEqual("PRODUTO 0001 Atualizado", produtoAtualizado.Codigo);
        }

        [TestMethod]
        public void QuandoReceberUmProdutoNovoDeveAdicionar()
        {
            _cadastroProduto.AtualizarProdutos(new List<ProdutoCadastroVm>()
                {
                    new ProdutoCadastroVm()
                        {
                            CodigoSap = "PROD0002",
                            Descricao = "PRODUTO 0002"
                        }
                });
            //garanto que adicionou porque não chamou o método de atualizar. Isto significa que foi chamado o construtor 
            //para criar uma nova instância
            //_produtoMock.Verify(x => x.AtualizaDescricao(It.IsAny<string>()), Times.Never());
            _atualizadorProdutoMock.Verify(x => x.Criar(It.IsAny<ProdutoCadastroVm>()),Times.Once());

        }



    }
}
