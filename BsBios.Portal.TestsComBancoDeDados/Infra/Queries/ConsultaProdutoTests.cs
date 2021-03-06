using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Queries
{
    [TestClass]
    public class ConsultaProdutoTests
    {
        [TestMethod]
        public void QuandoConsultarFornecedoresDoProdutoRetornaListaDeViewModels()
        {
            //preparando o cen�rios

            Fornecedor fornecedor01 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor02 = DefaultObjects.ObtemFornecedorPadrao();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            produto.AdicionarFornecedores(new List<Fornecedor>(){fornecedor01, fornecedor02});
            
            DefaultPersistedObjects.PersistirProduto(produto);

            var consultaProduto = ObjectFactory.GetInstance<IConsultaProduto>();

            var paginacaoVm = new PaginacaoVm
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };
            var kendoGridVm = consultaProduto.FornecedoresDoProduto(paginacaoVm, produto.Codigo);

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor01.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor02.Codigo));

        }

        [TestMethod]
        public void QuandoConsultaForneceedoresDeVariosProdutosRetornaListaDeTodosOFornecedoresVinculadosAosProdutosSemRepetir()
        {
            //cen�rio: dois produtos, tr�s fornecedores distintos. Sendo que um dos fornecedores est� vinculado a ambos produtos.
            //Os outros dois fornecedores, cada um est� vinculado a um produto.
            //Deve retornar uma lista com 3 fornecedores, sem repetir o fornecedor que est� compartilhado com os dois produtos.
            Produto produto1 = DefaultObjects.ObtemProdutoPadrao(); 
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedorCompartilhado = DefaultObjects.ObtemFornecedorPadrao();

            produto1.AdicionarFornecedores(new List<Fornecedor>{fornecedor1, fornecedorCompartilhado});
            produto2.AdicionarFornecedores(new List<Fornecedor> { fornecedor2, fornecedorCompartilhado });

            DefaultPersistedObjects.PersistirProduto(produto1);
            DefaultPersistedObjects.PersistirProduto(produto2);

            var consultaProduto = ObjectFactory.GetInstance<IConsultaProduto>();
            PaginacaoVm paginacaoVm = DefaultObjects.ObtemPaginacaoDefault();

            KendoGridVm kendoGridVm = consultaProduto.FornecedoresDosProdutos(paginacaoVm, new [] {produto1.Codigo, produto2.Codigo});

            Assert.AreEqual(3, kendoGridVm.QuantidadeDeRegistros);

            IList<FornecedorCadastroVm> registros = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.IsTrue(registros.Any(x => x.Codigo == fornecedor1.Codigo));
            Assert.IsTrue(registros.Any(x => x.Codigo == fornecedor2.Codigo));
            Assert.IsTrue(registros.Any(x => x.Codigo == fornecedorCompartilhado.Codigo)); 
        }
    }
}
