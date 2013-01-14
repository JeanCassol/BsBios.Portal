using BsBios.Portal.Infra.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests
{
    [TestClass]
    public class BaseTestClass
    {
        [AssemblyInitialize]
        public static void TesteInitialize(TestContext context)
        {
            //SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["conexaoPadrao"].ConnectionString);
            IoCWorker.Configure();
        }
    }
}
