using System;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class Produtos: RepositoryTest
    {
        private static IProdutos _produtos;

        //public Produtos()
        //{
        //    _produtos = ObjectFactory.GetInstance<IProdutos>();
        //}

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverProdutosCadastrados();
            _produtos = ObjectFactory.GetInstance<IProdutos>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoPersistoUmProdutoComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();

            var produto = new Produto("SAP0001", "PRODUTO 0001");
            _produtos.Save(produto);

            UnitOfWorkNh.Commit();

            Produto produtoConsulta = _produtos.BuscaPorCodigoSap("SAP0001");

            Assert.IsNotNull(produtoConsulta);
            Assert.AreEqual("SAP0001", produtoConsulta.CodigoSap);
        }

        [TestMethod]
        public void QuandoConsultoUmProdutoComCodigoSapInexistenteDeveRetornarNull()
        {
            Produto produto = _produtos.BuscaPorCodigoSap("SAP0002");
            Assert.IsNull(produto);
        }
    }
}
