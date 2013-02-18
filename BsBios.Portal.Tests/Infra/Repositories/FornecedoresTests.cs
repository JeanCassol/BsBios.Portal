using BsBios.Portal.Domain.Model;
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
            Queries.RemoverFornecedoresCadastrados();
            _fornecedores = ObjectFactory.GetInstance<IFornecedores>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }
        [TestMethod]
        public void QuandoPersistoUmFornecedorComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();

            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br");
            _fornecedores.Save(fornecedor);

            UnitOfWorkNh.Commit();

            var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

            Fornecedor fornecedorConsulta = fornecedores.BuscaPeloCodigoSap("FORNEC0001");


            Assert.IsNotNull(fornecedorConsulta);
            Assert.AreEqual("FORNEC0001", fornecedorConsulta.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedorConsulta.Nome);
            Assert.AreEqual("fornecedor@empresa.com.br", fornecedorConsulta.Email);
        }

        [TestMethod]
        public void ConsigoAlterarUmFornecedorCadastrado()
        {
            UnitOfWorkNh.BeginTransaction();
            var fornecedor = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor@empresa.com.br");
            _fornecedores.Save(fornecedor);
            UnitOfWorkNh.Commit();

            var fornecedorConsulta = _fornecedores.BuscaPeloCodigoSap("FORNEC0003");
            fornecedorConsulta.Atualizar("FORNECEDOR 0003 ALTERADO", "fornecedoralterado@empresa.com.br");

            UnitOfWorkNh.BeginTransaction();
            _fornecedores.Save(fornecedorConsulta);
            UnitOfWorkNh.Commit();

            var fornecedorConsultaAtualizacao = _fornecedores.BuscaPeloCodigoSap("FORNEC0003");
            Assert.AreEqual("FORNEC0003", fornecedorConsultaAtualizacao.Codigo);
            Assert.AreEqual("FORNECEDOR 0003 ALTERADO", fornecedorConsultaAtualizacao.Nome);
            Assert.AreEqual("fornecedoralterado@empresa.com.br", fornecedorConsultaAtualizacao.Email);

        }

        [TestMethod]
        public void QuandoConsultoUmFornecedorComCodigoSapInexistenteDeveRetornarNulo()
        {
            var fornecedor = _fornecedores.BuscaPeloCodigoSap("FORNEC0002");
            Assert.IsNull(fornecedor);
        }
    }
}
