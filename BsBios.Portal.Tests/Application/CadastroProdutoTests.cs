using System;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.ApplicationServices.Implementation;
using BsBios.Portal.Domain.Model;
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
        private readonly Mock<IUnitOfWorkNh> _unitOfWorkNhMock;
        private readonly Mock<IProdutos> _produtosMock;
        private readonly ProdutoCadastroVm _produtoPadrao;

        public CadastroProdutoTests()
        {
            _unitOfWorkNhMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>()));
            _cadastroProduto = new CadastroProduto(_unitOfWorkNhMock.Object, _produtosMock.Object);
            _produtoPadrao = new ProdutoCadastroVm()
                {
                    CodigoSap = "SAP 0001",
                    Descricao = "PRODUTO 0001" 
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
            _unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkNhMock.Verify(x => x.Commit(), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoFazRollback()
        {
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o produto"));
            try
            {
                _cadastroProduto.Novo(_produtoPadrao);
                Assert.Fail("Deveria ter gerado excessão");
                
            }
            catch(ExcecaoDeTeste)
            {
                _unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkNhMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkNhMock.Verify(x => x.Commit(), Times.Never());
            }
        }
    }
}
