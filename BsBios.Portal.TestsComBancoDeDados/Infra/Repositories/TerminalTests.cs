using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class TerminalTests: RepositoryTest
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
        public void ConsigoConsultarUmTerminalPeloCodigo()
        {
            //tenho dois terminais cadastrados: 1000 e 2000
            var terminais = new Terminais(ObjectFactory.GetInstance<IUnitOfWorkNh>());
            Terminal terminal = terminais.BuscaPeloCodigo("1000");

            Assert.IsNotNull(terminal);
            Assert.AreEqual("1000", terminal.Codigo);

        }
    }
}
