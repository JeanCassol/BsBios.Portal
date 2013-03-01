using BsBios.Portal.Infra.Repositories.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        protected static IUnitOfWorkNh UnitOfWorkNh;
        protected static ISession Session; 

        //[ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Session = ObjectFactory.GetInstance<ISession>();
            UnitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
        }

        //[ClassCleanup]
        public static void Cleanup()
        {
            UnitOfWorkNh.Dispose();
        }
    }
}
