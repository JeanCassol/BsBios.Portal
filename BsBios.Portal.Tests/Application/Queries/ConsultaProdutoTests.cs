using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaProdutoTests
    {
        [TestMethod]
        public void QuandoConsultarFornecedoresDoProdutoRetornaListaDeViewModels()
        {
            //preparando o cenários

            Fornecedor fornecedor01 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor02 = DefaultObjects.ObtemFornecedorPadrao();
            var produto = new Produto("PROD0001", "PRODUTO 0001", "01");
            produto.AdicionarFornecedores(new List<Fornecedor>(){fornecedor01, fornecedor02});
            
            var produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            produtosMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                        .Returns(produto);

            var consultaProduto = new ConsultaProduto(produtosMock.Object, new FornecedorCadastroBuilder(),new ProdutoCadastroBuilder());

            var kendoGridVm = consultaProduto.FornecedoresDoProduto("PROD0001");

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor01.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor02.Codigo));

            produtosMock.Verify(x => x.BuscaPeloCodigo(It.IsAny<string>()), Times.Once());

        }
    }
}
