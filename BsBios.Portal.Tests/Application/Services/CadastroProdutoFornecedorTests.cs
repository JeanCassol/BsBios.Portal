using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroProdutoFornecedorTests
    {
        private readonly Mock<IProdutos> _produtosMock;
        private readonly Mock<IFornecedores> _fornecedoresMock; 
        private readonly ICadastroProdutoFornecedor _cadastroProdutoFornecedor;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        
        private readonly Produto _produto;

        public CadastroProdutoFornecedorTests()
        {
            _produto = new Produto("PROD0001","PRODUTO 0001", "01");
            _produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            _produtosMock.Setup(x => x.BuscaPeloCodigo("PROD0001")).Returns(_produto);
            _produtosMock.Setup(x => x.Save(It.IsAny<Produto>()))
                         .Callback((Produto produto) =>
                         { 
                             Assert.IsNotNull(produto); 
                             Assert.IsTrue(produto.Fornecedores.All(p => p != null)); 
                         });

            _fornecedoresMock   = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.BuscaListaPorCodigo(new[] {"FORNEC0001", "FORNEC0002"}))
                             .Returns(_fornecedoresMock.Object);
            _fornecedoresMock.Setup(x => x.List())
                             .Returns(new List<Fornecedor>()
                                 {
                                     new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor01@empresa.com.br","","","",false, "Endereco 0001"),
                                     new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor02@empresa.com.br","","","", false, "Endereco 0002")
                                 });

            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _cadastroProdutoFornecedor = new CadastroProdutoFornecedor(_produtosMock.Object, _fornecedoresMock.Object, _unitOfWorkMock.Object);

        }
        
        [TestMethod]
        public void QuandoAtualizoListaDeFornecedoresDoProdutoOsNovosFornecedoresSaoAtualizados()
        {
            Assert.AreEqual(0, _produto.Fornecedores.Count);
            _cadastroProdutoFornecedor.AtualizarFornecedoresDoProduto("PROD0001", new[] { "FORNEC0001", "FORNEC0002" });
            Assert.AreEqual(2, _produto.Fornecedores.Count);

            _produtosMock.Verify(x => x.BuscaPeloCodigo("PROD0001"), Times.Once());
            _fornecedoresMock.Verify(x => x.BuscaListaPorCodigo(new[] {"FORNEC0001", "FORNEC0002"}), Times.Once());
            _fornecedoresMock.Verify(x => x.List(),Times.Once());
        }

        [TestMethod]
        public void QuandoAtualizoListaDeFornecedosDoProdutoOcorrePersistencia()
        {
            _cadastroProdutoFornecedor.AtualizarFornecedoresDoProduto("PROD0001", new[] { "FORNEC0001", "FORNEC0002" });
            _produtosMock.Verify(x => x.Save(It.IsAny<Produto>()),Times.Once());   

        }

        [TestMethod]
        public void QuandoAtualizoListaDeFornecedoresComSucessoERealizadoCommit()
        {
            _cadastroProdutoFornecedor.AtualizarFornecedoresDoProduto("PROD0001", new[] { "FORNEC0001", "FORNEC0002" });
            _unitOfWorkMock.Verify(x => x.BeginTransaction(),Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(),Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(),Times.Never());
        }

        [TestMethod]
        public void QuandoAtualizoListaDeFornecedoresComErroERealizadoRollBack()
        {
            _produtosMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                         .Throws(new Exception("Ocorreu um erro ao consulta o produto"));

            try
            {
                _cadastroProdutoFornecedor.AtualizarFornecedoresDoProduto("PROD0001", new[] { "FORNEC0001", "FORNEC0002" });
                Assert.Fail("Deveria ter gerado excessão");
            }
            catch (Exception)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
            }
        }
    }
}
