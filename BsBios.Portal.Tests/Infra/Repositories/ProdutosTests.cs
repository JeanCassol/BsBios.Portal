using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ProdutosTests: RepositoryTest
    {
        private static IProdutos _produtos;
        private static IFornecedores _fornecedores;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverProdutosCadastrados();
            _produtos = ObjectFactory.GetInstance<IProdutos>();
            _fornecedores = ObjectFactory.GetInstance<IFornecedores>();

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

            var produto = new Produto("SAP0001", "PRODUTO 0001", "01");
            _produtos.Save(produto);

            UnitOfWorkNh.Commit();

            Produto produtoConsulta = _produtos.BuscaPeloCodigo("SAP0001");

            Assert.IsNotNull(produtoConsulta);
            Assert.AreEqual("SAP0001", produtoConsulta.Codigo);
        }

        [TestMethod]
        public void QuandoConsultoUmProdutoComCodigoSapInexistenteDeveRetornarNull()
        {
            Produto produto = _produtos.BuscaPeloCodigo("SAP0002");
            Assert.IsNull(produto);
        }

        [TestMethod]
        public void QuandoPersistoUmProdutoComFornecedoresConsigoConsultarOsFornecedoresPosteriormente()
        {
            Queries.RemoverProdutosCadastrados();
            Queries.RemoverFornecedoresCadastrados();
            UnitOfWorkNh.BeginTransaction();

            var produto = new Produto("PROD0001", "Produto de Teste", "01");
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor01@empresa.com.br");
            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor02@empresa.com.br");
            _fornecedores.Save(fornecedor1);
            _fornecedores.Save(fornecedor2);
            var fornecedores = new List<Fornecedor>()
                {
                    fornecedor1 , fornecedor2
                };

            produto.AdicionarFornecedores(fornecedores);

            _produtos.Save(produto);

            UnitOfWorkNh.Commit();

            Produto produtoConsultado = _produtos.BuscaPeloCodigo("PROD0001");

            Assert.AreEqual(2, produtoConsultado.Fornecedores.Count);
        }
    }
}
