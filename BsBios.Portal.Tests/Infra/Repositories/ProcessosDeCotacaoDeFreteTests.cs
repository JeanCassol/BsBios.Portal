using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ProcessosDeCotacaoDeFreteTests:RepositoryTest
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
        public void ConsigoPersistirUmProcessoDeCotacaoDeFreteEConsultarPosteriormente()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            ProcessoDeCotacao processoDeCotacao = processosDeCotacao.BuscaPorId(processo.Id).Single();
            Assert.IsNotNull(processoDeCotacao);

        }
    }
}
