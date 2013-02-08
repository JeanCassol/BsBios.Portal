using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class CondicoesDePagamentoTests: RepositoryTest
    {
        private static ICondicoesDePagamento _condicoesDePagamento;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Queries.RemoverCondicoesDePagamentoCadastradas();
            _condicoesDePagamento = ObjectFactory.GetInstance<ICondicoesDePagamento>();
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void QuandoPersistoUmaCondicaoDePagamentoComSucessoConsigoConsultarPosteriormente()
        {
            UnitOfWorkNh.BeginTransaction();

            var condicaoDePagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");
            _condicoesDePagamento.Save(condicaoDePagamento);

            UnitOfWorkNh.Commit();

            CondicaoDePagamento condicaoDePagamentoConsulta = _condicoesDePagamento.BuscaPeloCodigoSap("C001");

            Assert.IsNotNull(condicaoDePagamentoConsulta);
            Assert.AreEqual("C001", condicaoDePagamentoConsulta.CodigoSap);
        }

        [TestMethod]
        public void QuandoConsultoUmCondicaoDePagamentoComCodigoSapInexistenteDeveRetornarNull()
        {
            CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigoSap("C002");
            Assert.IsNull(condicaoDePagamento);
        }
    }
}
