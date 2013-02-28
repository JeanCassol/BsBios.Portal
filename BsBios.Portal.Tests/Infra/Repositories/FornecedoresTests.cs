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

        [TestInitialize]
        public void InitializeTests()
        {
            Queries.RemoverFornecedoresCadastrados();
            Queries.RemoverProdutosCadastrados();
        }

        [TestMethod]
        public void QuandoPersistoUmFornecedorComSucessoConsigoConsultarPosteriormente()
        {
            try
            {
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
                UnitOfWorkNh.Session.Clear();
                UnitOfWorkNh.BeginTransaction();
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
            var fornecedor = _fornecedores.BuscaPeloCodigo("FORNEC0002");
            Assert.IsNull(fornecedor);
        }

        [TestMethod]
        public void QuandoCarregarPorListaDeCodigosTemQueCarregarFornecedoresEquivalentesALista()
        {
            try
            {
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

        [TestMethod]
        public void QuandoConsultarFornecedoresNaoVinculadosAoProdutoNenhumDosFornecedoresRetornadosEstaVinculadoAoProduto()
        {
            UnitOfWorkNh.BeginTransaction();
            var produto = new Produto("PROD0001", "PRODUTO 0001", "01");
            //CRIA 4 FORNECEDORES
            var fornecedor01 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br");
            var fornecedor02 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0002@empresa.com.br");
            var fornecedor03 = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor0003@empresa.com.br");
            var fornecedor04 = new Fornecedor("FORNEC0004", "FORNECEDOR 0004", "fornecedor0004@empresa.com.br");
            //VINCULA FORNECEDORES 1 E 2 AO PRODUTO. OS FORNECEDORES 3 E 4 NÃO SERÃO VINCULADOS
            produto.AdicionarFornecedores(new List<Fornecedor> { fornecedor01, fornecedor02 });
            UnitOfWorkNh.Session.Save(produto);
            UnitOfWorkNh.Session.Save(fornecedor03);
            UnitOfWorkNh.Session.Save(fornecedor04);
            UnitOfWorkNh.Commit();

            UnitOfWorkNh.Session.Clear();

            _fornecedores.FornecedoresNaoVinculadosAoProduto("PROD0001");
            Assert.AreEqual(2, _fornecedores.Count());
            IList < Fornecedor > fornecedoresNaoVinculados = _fornecedores.List();
            Assert.AreEqual(1,fornecedoresNaoVinculados.Count(x => x.Codigo == "FORNEC0003") );
            Assert.AreEqual(1, fornecedoresNaoVinculados.Count(x => x.Codigo == "FORNEC0004"));

        }
    }
}
