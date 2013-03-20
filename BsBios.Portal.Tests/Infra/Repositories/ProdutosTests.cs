using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using System.Linq;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ProdutosTests: RepositoryTest
    {
        private static IFornecedores _fornecedores;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
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
            Produto produto;
            try
            {
                UnitOfWorkNh.BeginTransaction();
                produto = DefaultObjects.ObtemProdutoPadrao();
                UnitOfWorkNh.Session.Save(produto);

                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            var produtos = ObjectFactory.GetInstance<IProdutos>();
            Produto produtoConsultado = produtos.BuscaPeloCodigo(produto.Codigo);

            Assert.IsNotNull(produtoConsultado);
            Assert.AreEqual(produto.Codigo, produtoConsultado.Codigo);
            Assert.AreEqual(produto.Descricao, produtoConsultado.Descricao);
        }

        [TestMethod]
        public void QuandoConsultoUmProdutoComCodigoSapInexistenteDeveRetornarNull()
        {
            var produtos = ObjectFactory.GetInstance<IProdutos>();
            Produto produto = produtos.BuscaPeloCodigo("SAP0002");
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

                UnitOfWorkNh.Session.Save(produto);

                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            var produtos = ObjectFactory.GetInstance<IProdutos>();
            Produto produtoConsultado = produtos.BuscaPeloCodigo(produto.Codigo);

            Assert.AreEqual(2, produtoConsultado.Fornecedores.Count);
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeCodigoTemQueRetornarProdutosCorrespondenteAosCodigos()
        {
            UnitOfWorkNh.BeginTransaction();
            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            Produto produto3 = DefaultObjects.ObtemProdutoPadrao();
            UnitOfWorkNh.Session.Save(produto1);
            UnitOfWorkNh.Session.Save(produto2);
            UnitOfWorkNh.Session.Save(produto3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var produtos = ObjectFactory.GetInstance<IProdutos>();
            IList<Produto> produtosConsultados = produtos.FiltraPorListaDeCodigos(new[] { produto1.Codigo, produto2.Codigo }).List();

            Assert.AreEqual(2, produtosConsultados.Count);
            Assert.AreEqual(1, produtosConsultados.Count(x => x.Codigo == produto1.Codigo));
            Assert.AreEqual(1, produtosConsultados.Count(x => x.Codigo == produto2.Codigo));
        }

        [TestMethod]
        public void ConsigoConsultarDoisProdutosSeguidosPeloCodigoUtilizandoAMesmaInstanciaDoRepositorio()
        {
            UnitOfWorkNh.BeginTransaction();
            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            UnitOfWorkNh.Session.Save(produto1);
            UnitOfWorkNh.Session.Save(produto2);
            UnitOfWorkNh.Commit();

            UnitOfWorkNh.Session.Clear();

            var produtos = ObjectFactory.GetInstance<IProdutos>();
            Produto produto1Consultado = produtos.BuscaPeloCodigo(produto1.Codigo);
            Produto produto2Consultado = produtos.BuscaPeloCodigo(produto2.Codigo);
            Assert.AreEqual(produto1Consultado.Codigo, produto1.Codigo);
            Assert.AreEqual(produto1Consultado.Descricao, produto1.Descricao);
            Assert.AreEqual(produto1Consultado.Tipo, produto1.Tipo);

            Assert.AreEqual(produto2Consultado.Codigo, produto2.Codigo);
            Assert.AreEqual(produto2Consultado.Descricao, produto2.Descricao);
            Assert.AreEqual(produto2Consultado.Tipo, produto2.Tipo);

        }

        [TestMethod]
        public void QuandoInstancioProdutosDeFreteCarregaOsTiposCorretamenteDoArquivoDeConfiguracao()
        {
            var produtosDeFrete = ObjectFactory.GetInstance<IProdutosDeFrete>();
            Assert.AreEqual("FERT,NLAG,ROH,YO3R,YOAG,ZROH", produtosDeFrete.TiposDeProdutoDeFrete);
        }
    }
}
