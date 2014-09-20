using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class CondicoesDePagamentoTests: RepositoryTest
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
        public void QuandoPersistoUmaCondicaoDePagamentoComSucessoConsigoConsultarPosteriormente()
        {
            try
            {
                UnitOfWorkNh.BeginTransaction();

                var condicaoDePagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");
                UnitOfWorkNh.Session.Save(condicaoDePagamento);

                UnitOfWorkNh.Commit();

            }
            catch (Exception)
            {
                UnitOfWorkNh.RollBack();
                throw;
            }

            var condicoesDePagamento = ObjectFactory.GetInstance<ICondicoesDePagamento>();
            CondicaoDePagamento condicaoDePagamentoConsulta = condicoesDePagamento.BuscaPeloCodigo("C001");

            Assert.IsNotNull(condicaoDePagamentoConsulta);
            Assert.AreEqual("C001", condicaoDePagamentoConsulta.Codigo);
        }

        [TestMethod]
        public void QuandoConsultoUmaCondicaoDePagamentoComCodigoSapInexistenteDeveRetornarNull()
        {
            var condicoesDePagamento = ObjectFactory.GetInstance<ICondicoesDePagamento>();
            CondicaoDePagamento condicaoDePagamento = condicoesDePagamento.BuscaPeloCodigo("C002");
            Assert.IsNull(condicaoDePagamento);
        }

        [TestMethod]
        public void QuandoFiltroPorListaDeCodigoTemQueRetornarIncotermsCorrespondenteAosCodigos()
        {
            UnitOfWorkNh.BeginTransaction();
            CondicaoDePagamento condicaoDePagamento1 = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            CondicaoDePagamento condicaoDePagamento2 = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            CondicaoDePagamento condicaoDePagamento3 = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            UnitOfWorkNh.Session.Save(condicaoDePagamento1);
            UnitOfWorkNh.Session.Save(condicaoDePagamento2);
            UnitOfWorkNh.Session.Save(condicaoDePagamento3);
            UnitOfWorkNh.Commit();
            UnitOfWorkNh.Session.Clear();

            var condicoesDePagamento = ObjectFactory.GetInstance<ICondicoesDePagamento>();

            IList<CondicaoDePagamento> condicoesConsultadas = condicoesDePagamento.FiltraPorListaDeCodigos(new[] { condicaoDePagamento1.Codigo, condicaoDePagamento2.Codigo }).List();
            Assert.AreEqual(2, condicoesConsultadas.Count);
            Assert.AreEqual(1, condicoesConsultadas.Count(x => x.Codigo == condicaoDePagamento1.Codigo));
            Assert.AreEqual(1, condicoesConsultadas.Count(x => x.Codigo == condicaoDePagamento2.Codigo));

        }
    }
}
