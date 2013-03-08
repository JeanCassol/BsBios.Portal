using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
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
            //Queries.RemoverProdutosCadastrados();
            _produtos = ObjectFactory.GetInstance<IProdutos>();
            _fornecedores = ObjectFactory.GetInstance<IFornecedores>();

        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        private void RemoverFornecedoresCadastrados()
        {
            var fornecedores = _fornecedores.List();
            foreach (var fornecedor in fornecedores)
            {
                _fornecedores.Delete(fornecedor);
            }

        }

        private void RemoverProdutosCadastrados()
        {
            var produtos = ObjectFactory.GetInstance<IProdutos>();
            var todosProdutos = produtos.List();
            foreach (var produto in todosProdutos)
            {
                produtos.Delete(produto);
            }
        }
        [TestMethod]
        public void QuandoPersistoUmProdutoComSucessoConsigoConsultarPosteriormente()
        {
            Produto produto;
            try
            {
                UnitOfWorkNh.BeginTransaction();
                produto = DefaultObjects.ObtemProdutoPadrao();
                _produtos.Save(produto);

                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            Produto produtoConsultado = _produtos.BuscaPeloCodigo(produto.Codigo);

            Assert.IsNotNull(produtoConsultado);
            Assert.AreEqual(produto.Codigo, produtoConsultado.Codigo);
            Assert.AreEqual(produto.Descricao, produtoConsultado.Descricao);
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
            Produto produto;
            try
            {
                UnitOfWorkNh.BeginTransaction();
                produto = DefaultObjects.ObtemProdutoPadrao();
                Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
                Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
                _fornecedores.Save(fornecedor1);
                _fornecedores.Save(fornecedor2);
                var fornecedores = new List<Fornecedor>()
                {
                    fornecedor1 , fornecedor2
                };

                produto.AdicionarFornecedores(fornecedores);

                _produtos.Save(produto);

                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            Produto produtoConsultado = _produtos.BuscaPeloCodigo(produto.Codigo);

            Assert.AreEqual(2, produtoConsultado.Fornecedores.Count);
        }
    }
}
