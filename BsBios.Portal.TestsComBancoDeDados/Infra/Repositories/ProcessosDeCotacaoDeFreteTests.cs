using System;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
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

        /// <summary>
        /// Este teste verifica se quando salvo um processo de cotação, as cotações adicionadas também são salvas
        /// e podem ser consultadas posteriormente
        /// </summary>
        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComCotacoes()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processo.AdicionarFornecedor(fornecedor);

            processo.Abrir();
            processo.InformarCotacao(fornecedor.Codigo, 120, 100, "obs fornec");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacaoDeFrete = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeFrete)processosDeCotacaoDeFrete.BuscaPorId(processo.Id).Single();
            Assert.AreEqual(processo.FornecedoresParticipantes.Count(x => x.Cotacao != null), processoConsultado.FornecedoresParticipantes.Count(x => x.Cotacao != null));
            Console.WriteLine("Consultando Cotacao - FIM");
        }
    }
}
