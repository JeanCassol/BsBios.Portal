using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class FornecedoresTests:RepositoryTest
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

       // private void RemoverFornecedoresCadastrados()
       // {
       //     UnitOfWorkNh.BeginTransaction();
       //     var fornecedores = _fornecedores.List();
       //     foreach (var fornecedor in fornecedores)
       //     {
       //         _fornecedores.Delete(fornecedor);
       //     }
       //     //UnitOfWorkNh.Session.Flush();
       //     UnitOfWorkNh.Commit();
       //     UnitOfWorkNh.Session.Clear();
       // }

       // private void RemoverProdutosCadastrados()
       // {
       //     UnitOfWorkNh.BeginTransaction();
       //     var produtos = ObjectFactory.GetInstance<IProdutos>();
       //     var todosProdutos = produtos.List();
       //     foreach (var produto in todosProdutos)
       //     {
       //         produtos.Delete(produto);
       //     }        
       //     UnitOfWorkNh.Commit();
       //     UnitOfWorkNh.Session.Clear();
       //}


        //[TestInitialize]
        public void InitializeTests()
        {
            //Queries.RemoverFornecedoresCadastrados();
            //UnitOfWorkNh.BeginTransaction();
            //var fornecedores = _fornecedores.List();
            //foreach (var fornecedor in fornecedores)
            //{
            //    _fornecedores.Delete(fornecedor);
            //}
            //UnitOfWorkNh.Commit();
        }

        [TestMethod]
        public void QuandoPersistoUmFornecedorComSucessoConsigoConsultarPosteriormente()
        {
            try
            {
                Queries.RemoverFornecedoresCadastrados();
                Queries.RemoverProdutosCadastrados();
                UnitOfWorkNh.BeginTransaction();
                UnitOfWorkNh.Session.Clear();
                var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br");
                _fornecedores.Save(fornecedor);
                UnitOfWorkNh.Commit();
            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }
            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

            Fornecedor fornecedorConsulta = fornecedores.BuscaPeloCodigo("FORNEC0001");

            Assert.IsNotNull(fornecedorConsulta);
            Assert.AreEqual("FORNEC0001", fornecedorConsulta.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedorConsulta.Nome);
            Assert.AreEqual("fornecedor@empresa.com.br", fornecedorConsulta.Email);
        }

        [TestMethod]
        public void ConsigoAlterarUmFornecedorCadastrado()
        {
            try
            {
                Queries.RemoverFornecedoresCadastrados();
                Queries.RemoverProdutosCadastrados();

                UnitOfWorkNh.BeginTransaction();
                UnitOfWorkNh.Session.Clear();
                var fornecedor = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor@empresa.com.br");
                _fornecedores.Save(fornecedor);
                Console.WriteLine("INSERIDO FORNEC0003");
                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }
            try
            {
                UnitOfWorkNh.BeginTransaction();
                var fornecedorConsulta = _fornecedores.BuscaPeloCodigo("FORNEC0003");
                fornecedorConsulta.Atualizar("FORNECEDOR 0003 ALTERADO", "fornecedoralterado@empresa.com.br");

                _fornecedores.Save(fornecedorConsulta);
                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            var fornecedorConsultaAtualizacao = _fornecedores.BuscaPeloCodigo("FORNEC0003");
            Assert.AreEqual("FORNEC0003", fornecedorConsultaAtualizacao.Codigo);
            Assert.AreEqual("FORNECEDOR 0003 ALTERADO", fornecedorConsultaAtualizacao.Nome);
            Assert.AreEqual("fornecedoralterado@empresa.com.br", fornecedorConsultaAtualizacao.Email);

        }

        [TestMethod]
        public void QuandoConsultoUmFornecedorComCodigoSapInexistenteDeveRetornarNulo()
        {
            Queries.RemoverFornecedoresCadastrados();
            var fornecedor = _fornecedores.BuscaPeloCodigo("FORNEC0002");
            Assert.IsNull(fornecedor);
        }

        [TestMethod]
        public void QuandoCarregarPorListaDeCodigosTemQueCarregarFornecedoresEquivalentesALista()
        {
            try
            {
                Queries.RemoverFornecedoresCadastrados();
                Queries.RemoverProdutosCadastrados();
                UnitOfWorkNh.BeginTransaction();
                UnitOfWorkNh.Session.Clear();
                var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor01@empresa.com.br");
                var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0003", "fornecedor02@empresa.com.br");
                var fornecedor3 = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor03@empresa.com.br");

                _fornecedores.Save(fornecedor1);
                _fornecedores.Save(fornecedor2);
                _fornecedores.Save(fornecedor3);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();                
                throw;
            }

            var codigoDosFornecedores = new[] {"FORNEC0001", "FORNEC0002"};
            IList < Fornecedor > fornecedores = _fornecedores.BuscaListaPorCodigo(codigoDosFornecedores).List();
            Assert.AreEqual(codigoDosFornecedores.Length, fornecedores.Count);
            foreach (var codigoDoFornecedor in codigoDosFornecedores)
            {
                Assert.IsNotNull(fornecedores.SingleOrDefault(x => x.Codigo == codigoDoFornecedor));
            }

        }
    }
}
