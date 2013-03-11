using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
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

            var fornecedor01 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br");
            var fornecedor02 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0002@empresa.com.br");
            var produto = new Produto("PROD0001", "PRODUTO 0001", "01");
            produto.AdicionarFornecedores(new List<Fornecedor>(){fornecedor01, fornecedor02});
            
            var produtosMock = new Mock<IProdutos>(MockBehavior.Strict);
            produtosMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                        .Returns(produto);

            var consultaProduto = new ConsultaProduto(produtosMock.Object, new FornecedorCadastroBuilder(),new ProdutoCadastroBuilder());

            var kendoGridVm = consultaProduto.FornecedoresDoProduto("PROD0001");

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == "FORNEC0001"));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == "FORNEC0002"));

            produtosMock.Verify(x => x.BuscaPeloCodigo(It.IsAny<string>()), Times.Once());

        }
    }
}
