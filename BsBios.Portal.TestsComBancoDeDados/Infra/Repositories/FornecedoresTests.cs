﻿using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
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
                RollbackSessionTransaction();
                throw;
            }

            Session.Clear();

            Fornecedor fornecedorConsulta = fornecedores.BuscaPeloCodigo(fornecedor.Codigo);

            Assert.IsNotNull(fornecedorConsulta);
            Assert.AreEqual(fornecedor.Codigo, fornecedorConsulta.Codigo);
            Assert.AreEqual("FORNECEDOR " + fornecedor.Codigo, fornecedorConsulta.Nome);
            Assert.AreEqual("fornecedor"+ fornecedor.Codigo +  "@empresa.com.br", fornecedorConsulta.Email);
            Assert.AreEqual("cnpj" + fornecedor.Codigo, fornecedor.Cnpj);
            Assert.AreEqual("municipio" + fornecedor.Codigo, fornecedorConsulta.Municipio);
            Assert.AreEqual("uf", fornecedorConsulta.Uf);
        }

        [TestMethod]
        public void ConsigoAlterarUmFornecedorCadastrado()
        {
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            DefaultPersistedObjects.PersistirFornecedor(fornecedor);

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            UnitOfWorkNh.Session.Clear();
            try
            {
                UnitOfWorkNh.BeginTransaction();

                var fornecedorConsulta = fornecedores.BuscaPeloCodigo(fornecedor.Codigo);
                fornecedorConsulta.Atualizar("FORNECEDOR ALTERADO", "fornecedoralterado@empresa.com.br", "cnpj alterado", "municipio alterado", "uf",false);
                fornecedores.Save(fornecedorConsulta);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();            
                throw;
            }

            UnitOfWorkNh.Session.Clear();

            var fornecedorConsultaAtualizacao = fornecedores.BuscaPeloCodigo(fornecedor.Codigo);
            Assert.AreEqual(fornecedor.Codigo, fornecedorConsultaAtualizacao.Codigo);
            Assert.AreEqual("FORNECEDOR ALTERADO", fornecedorConsultaAtualizacao.Nome);
            Assert.AreEqual("fornecedoralterado@empresa.com.br", fornecedorConsultaAtualizacao.Email);
            Assert.AreEqual("cnpj alterado",fornecedorConsultaAtualizacao.Cnpj);
            Assert.AreEqual("municipio alterado", fornecedorConsultaAtualizacao.Municipio);
            Assert.AreEqual("uf", fornecedorConsultaAtualizacao.Uf);

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
            RemoveQueries.RemoverProdutosCadastrados();
            RemoveQueries.RemoverFornecedoresCadastrados();
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

        [TestMethod]
        public void QuandoConsultaFornecedoresNaoVinculadosAUmaListaDeProdutosNenhumDosFornecedoresRetornadosEstaVinculadoAAlgumProdutoDaLista()
        {
            RemoveQueries.RemoverProdutosCadastrados();
            RemoveQueries.RemoverFornecedoresCadastrados();
            //cenário:
            //três fornecedores e três produtos: Os fornecedores 1 e 2 estão ligados aos produtos 1 e 2. Já o fornecedor 3 está ligado ao produto 3 . 
            //vou passsar na lista de produtos, apenas os produtos 1 e 2. Deve retornar na lista de fornecedores apenas o fornecedor 3, que embora
            //esteja ligado ao produto 3, não está ligado a nenhum dos produtos da lista (1, 2).
            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            Produto produto3 = DefaultObjects.ObtemProdutoPadrao();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();

            produto1.AdicionarFornecedores(new List<Fornecedor>{fornecedor1});
            produto2.AdicionarFornecedores(new List<Fornecedor> { fornecedor2 });
            produto3.AdicionarFornecedores(new List<Fornecedor> { fornecedor3 });

            DefaultPersistedObjects.PersistirProduto(produto1);
            DefaultPersistedObjects.PersistirProduto(produto2);
            DefaultPersistedObjects.PersistirProduto(produto3);

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            IList<Fornecedor> fornecedoresNaoVinculados =
                fornecedores.FornecedoresNaoVinculadosAosProdutos(new[] {produto1.Codigo, produto2.Codigo}).List();

            Assert.AreEqual(1, fornecedoresNaoVinculados.Count);
            Assert.IsTrue(fornecedoresNaoVinculados.Any(x => x.Codigo == fornecedor3.Codigo));

        }

        [TestMethod]
        public void QuandoFiltraPorNomeRetornaFornecedoresQueContemOTextoNoSeuNome()
        {
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
            fornecedor2.Atualizar("MAURO SÉRGIO DA COSTA LEAL", fornecedor2.Email,"","","", true);
            fornecedor3.Atualizar("ANTONIO COSTA E SILVA", fornecedor3.Email, "", "", "", true);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor1);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor2);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor3);

            UnitOfWorkNh.Session.Clear();

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
            IList<Fornecedor> fornecedoresFiltrados = fornecedores.NomeContendo("costa").List();

            Assert.AreEqual(2, fornecedoresFiltrados.Count);

        }

        [TestMethod]
        public void QuandoFiltraSomenteTransportadorasTodosFornecedoresListadosSaoTransportadoras()
        {
            RemoveQueries.RemoverFornecedoresCadastrados();
            Fornecedor naoTransportadora = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor transportadora = DefaultObjects.ObtemTransportadoraPadrao();
            DefaultPersistedObjects.PersistirFornecedor(naoTransportadora);
            DefaultPersistedObjects.PersistirFornecedor(transportadora);

            UnitOfWorkNh.Session.Clear();

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

            IList<Fornecedor> transportadoras = fornecedores.SomenteTransportadoras().List();

            Assert.AreEqual(1, transportadoras.Count );

            Assert.IsTrue(transportadoras.First().Transportadora);


        }
        [TestMethod]
        public void QuandoFiltraSomenteFornecedoresQueNaoSaoDeTransporteTodosFornecedoresListadosNaoSaoDeTransporte()
        {
            RemoveQueries.RemoverFornecedoresCadastrados();
            Fornecedor naoTransportadora = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor transportadora = DefaultObjects.ObtemTransportadoraPadrao();
            DefaultPersistedObjects.PersistirFornecedor(naoTransportadora);
            DefaultPersistedObjects.PersistirFornecedor(transportadora);

            UnitOfWorkNh.Session.Clear();

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

            IList<Fornecedor> naoTransportadoras = fornecedores.RemoveTransportadoras().List();

            Assert.AreEqual(1, naoTransportadoras.Count);

            Assert.IsFalse(naoTransportadoras.First().Transportadora);
            
        }

    }
}
