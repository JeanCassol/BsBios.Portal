using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        protected static IUnitOfWorkNh UnitOfWorkNh;

        //[ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            UnitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        }

        //[ClassCleanup]
        public static void Cleanup()
        {
            UnitOfWorkNh.Dispose();
        }
    }
}
