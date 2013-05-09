using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaFornecedorTests: RepositoryTest
    {

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }


        [TestMethod]
        public void QuandoConsultarFornecedoresNaoVinculadosRetornaListaDeFornecedores()
        {
            RemoveQueries.RemoverFornecedoresCadastrados();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            Fornecedor fornecedor01 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor02 = DefaultObjects.ObtemFornecedorPadrao();
            produto.AdicionarFornecedores(new List<Fornecedor>{fornecedor01, fornecedor02});
            Fornecedor fornecedor03 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor04 = DefaultObjects.ObtemFornecedorPadrao();

            DefaultPersistedObjects.PersistirProduto(produto);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor03);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor04);

            UnitOfWorkNh.Session.Clear();

            var consultaFornecedores = ObjectFactory.GetInstance<IConsultaFornecedor>();

            var paginacaoVm = new PaginacaoVm()
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };

            var filtro = new FornecedorDoProdutoFiltro()
                {
                    CodigoProduto = produto.Codigo
                };
            var kendoGridVm = consultaFornecedores.FornecedoresNaoVinculadosAoProduto(paginacaoVm, filtro);

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor03.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor04.Codigo));

        }

        [TestMethod]
        public void QuandoFiltraFornecedoresPorNomeRetornaListaEContabemDeRegistrosCorreta()
        {
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
            fornecedor2.Atualizar("CARLOS EDUARDO DA SILVA", fornecedor2.Email,"","","", false);
            fornecedor3.Atualizar("LUIS EDUARDO SILVA", fornecedor3.Email,"","","",false);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor1);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor2);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor3);

            var consultaFornecedor = ObjectFactory.GetInstance<IConsultaFornecedor>();

            var paginacaoVm = new PaginacaoVm()
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };
            var filtro = new FornecedorFiltroVm
                {
                    Nome = "eduardo"
                };
            KendoGridVm kendoGridVm = consultaFornecedor.Listar(paginacaoVm, filtro);

            Assert.AreEqual(2,kendoGridVm.QuantidadeDeRegistros);

        }


    }
}
