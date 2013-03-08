using System;
using BsBios.Portal.Domain.Entities;
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
            try
            {
                UnitOfWorkNh.BeginTransaction();

                var condicaoDePagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");
                _condicoesDePagamento.Save(condicaoDePagamento);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();
                throw;
            }

            CondicaoDePagamento condicaoDePagamentoConsulta = _condicoesDePagamento.BuscaPeloCodigo("C001");

            Assert.IsNotNull(condicaoDePagamentoConsulta);
            Assert.AreEqual("C001", condicaoDePagamentoConsulta.Codigo);
        }

        [TestMethod]
        public void QuandoConsultoUmaCondicaoDePagamentoComCodigoSapInexistenteDeveRetornarNull()
        {
            CondicaoDePagamento condicaoDePagamento = _condicoesDePagamento.BuscaPeloCodigo("C002");
            Assert.IsNull(condicaoDePagamento);
        }
    }
}
