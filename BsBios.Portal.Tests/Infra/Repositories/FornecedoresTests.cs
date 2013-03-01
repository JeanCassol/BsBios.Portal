using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class FornecedoresTests:RepositoryTest
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
        public void QuandoPersistoUmFornecedorComSucessoConsigoConsultarPosteriormente()
        {
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            Fornecedor fornecedor;
            try
            {
                Session.BeginTransaction();
                fornecedor = DefaultObjects.ObtemFornecedorPadrao();

                Session.Save(fornecedor);
                Session.Transaction.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }
            Fornecedor fornecedorConsulta = fornecedores.BuscaPeloCodigo(fornecedor.Codigo);

            Assert.IsNotNull(fornecedorConsulta);
            Assert.AreEqual(fornecedor.Codigo, fornecedorConsulta.Codigo);
            Assert.AreEqual("FORNECEDOR " + fornecedor.Codigo, fornecedorConsulta.Nome);
            Assert.AreEqual("fornecedor"+ fornecedor.Codigo +  "@empresa.com.br", fornecedorConsulta.Email);
        }

        [TestMethod]
        public void ConsigoAlterarUmFornecedorCadastrado()
        {
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            string codigoFornedor;
            try
            {
                Session.BeginTransaction();
                var fornecedor = DefaultObjects.ObtemFornecedorPadrao();

                codigoFornedor = fornecedor.Codigo;
                Session.Save(fornecedor);
                Session.Transaction.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }
            try
            {
                UnitOfWorkNh.BeginTransaction();
                var fornecedorConsulta = fornecedores.BuscaPeloCodigo(codigoFornedor);
                fornecedorConsulta.Atualizar("FORNECEDOR ALTERADO", "fornecedoralterado@empresa.com.br");

                fornecedores.Save(fornecedorConsulta);
                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            var fornecedorConsultaAtualizacao = fornecedores.BuscaPeloCodigo(codigoFornedor);
            Assert.AreEqual(codigoFornedor, fornecedorConsultaAtualizacao.Codigo);
            Assert.AreEqual("FORNECEDOR ALTERADO", fornecedorConsultaAtualizacao.Nome);
            Assert.AreEqual("fornecedoralterado@empresa.com.br", fornecedorConsultaAtualizacao.Email);

        }

        [TestMethod]
        public void QuandoConsultoUmFornecedorComCodigoSapInexistenteDeveRetornarNulo()
        {
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            var fornecedor = fornecedores.BuscaPeloCodigo("__FORNEC0002");
            Assert.IsNull(fornecedor);
        }

        [TestMethod]
        public void QuandoCarregarPorListaDeCodigosTemQueCarregarFornecedoresEquivalentesALista()
        {
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            string[] codigoDosFornecedores;
            try
            {
                Session.BeginTransaction();
                Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
                Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
                Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();

                Session.Save(fornecedor1);
                Session.Save(fornecedor2);
                Session.Save(fornecedor3);

                codigoDosFornecedores = new[] { fornecedor1.Codigo, fornecedor2.Codigo };

                Session.Transaction.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }
            
            IList <Fornecedor> fornecedoresConsulta = fornecedores.BuscaListaPorCodigo(codigoDosFornecedores).List();
            Assert.AreEqual(codigoDosFornecedores.Length, fornecedoresConsulta.Count);
            foreach (var codigoDoFornecedor in codigoDosFornecedores)
            {
                Assert.IsNotNull(fornecedoresConsulta.SingleOrDefault(x => x.Codigo == codigoDoFornecedor));
            }

        }

        [TestMethod]
        public void QuandoConsultarFornecedoresNaoVinculadosAoProdutoNenhumDosFornecedoresRetornadosEstaVinculadoAoProduto()
        {
            Queries.RemoverProdutosCadastrados();
            Queries.RemoverFornecedoresCadastrados();
            Session.BeginTransaction();
            var produto = DefaultObjects.ObtemProdutoPadrao();
            //CRIA 4 FORNECEDORES
            var fornecedor01 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor02 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor03 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor04 = DefaultObjects.ObtemFornecedorPadrao();
            //VINCULA FORNECEDORES 1 E 2 AO PRODUTO. OS FORNECEDORES 3 E 4 NÃO SERÃO VINCULADOS
            produto.AdicionarFornecedores(new List<Fornecedor> { fornecedor01, fornecedor02 });
            Session.Save(produto);
            Session.Save(fornecedor03);
            Session.Save(fornecedor04);
            Session.Transaction.Commit();

            UnitOfWorkNh.Session.Clear();

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            fornecedores.FornecedoresNaoVinculadosAoProduto(produto.Codigo);
            Assert.AreEqual(2, fornecedores.Count());
            IList < Fornecedor > fornecedoresNaoVinculados = fornecedores.List();
            Assert.AreEqual(1,fornecedoresNaoVinculados.Count(x => x.Codigo == fornecedor03.Codigo) );
            Assert.AreEqual(1, fornecedoresNaoVinculados.Count(x => x.Codigo == fornecedor04.Codigo));

        }
    }
}
