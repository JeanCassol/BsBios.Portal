//using System.Linq;
//using BsBios.Portal.Domain.Entities;
//using BsBios.Portal.Infra.Repositories.Contracts;
//using BsBios.Portal.Tests.DataProvider;
//using BsBios.Portal.Tests.DefaultProvider;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using StructureMap;

//namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
//{
//    //[TestClass]
//    public class AgendamentosDeDescarregamentoTests: RepositoryTest
//    {
//        [ClassInitialize]
//        public static void Inicializar(TestContext testContext)
//        {
//            Initialize(testContext);
//        }
//        [ClassCleanup]
//        public static void Finalizar()
//        {
//            Cleanup();
//        }

//        [TestMethod]
//        public void ConsigoPersistirUmAgendamentoDeDescarregamentoEConsultarPosteriormente()
//        {
//            AgendamentoDeDescarregamento agendamento = DefaultObjects.ObtemAgendamentoDeDescarregamento();
//            NotaFiscal notaFiscal = agendamento.NotasFiscais.First();
//            DefaultPersistedObjects.PersistirAgendamentoDeCarga(agendamento);

//            UnitOfWorkNh.Session.Clear();

//            var agendamentos = ObjectFactory.GetInstance<IAgendamentosDeCarga>();
//            var agendamentoConsultado = (AgendamentoDeDescarregamento) agendamentos.BuscaPorId(agendamento.Id);

//            Assert.IsNotNull(agendamentoConsultado);
//            //Assert.AreEqual(agendamento.Material, agendamentoConsultado.Material);
//            //Assert.AreEqual(agendamento.Data, agendamentoConsultado.Data);
//            Assert.AreEqual(agendamento.Placa, agendamentoConsultado.Placa);
//            Assert.AreEqual(agendamento.Realizado, agendamentoConsultado.Realizado);

//            Assert.AreEqual(1, agendamentoConsultado.NotasFiscais.Count);

//        }

//    }
//}
