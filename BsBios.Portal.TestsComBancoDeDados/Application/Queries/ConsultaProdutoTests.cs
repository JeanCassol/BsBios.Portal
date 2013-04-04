using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
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
    }
}
