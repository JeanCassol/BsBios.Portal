using System.Configuration;
using BsBios.Portal.Infra.DataAccess;
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
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBiosTesteUnitario"].ConnectionString);
            IoCWorker.Configure();
        }
    }
}
