using System;
using System.Data;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.InfraServices.Repositories
{
    [TestClass]
    public class UnitOfWorkTests
    {
        [TestMethod]
        public void ConsigoInstanciarUnitOfWorkNhComObjectFactory()
        {
            var unitOfWork = ObjectFactory.GetInstance<IUnitOfWork>();
            Assert.IsInstanceOfType(unitOfWork, typeof(UnitOfWorkNh));
        }

        [TestMethod]
        public void ConsigoMeConectarNoBancoDeDados()
        {
            var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            Assert.AreEqual(ConnectionState.Open, unitOfWorkNh.Session.Connection.State);
        }


    }
}
