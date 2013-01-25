using System.Data;
using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
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
        public void QuandoInstancioUnitOfWorkGeneriicaEEspecificaRetornaMesmaInstancia()
        {
            var unitOfWorkGenerica = ObjectFactory.GetInstance<IUnitOfWork>();
            var unitOfWorkEspecifica = ObjectFactory.GetInstance<UnitOfWorkNh>();
            Assert.AreSame(unitOfWorkEspecifica, unitOfWorkGenerica);
        }

        //esse teste não devia estar aqui. Só quis ter certeza que cada vez que o StructureMap
        //instancia um objeto que está configurado para ser instanciado por requisição retornar
        //objetos diferentes
        [TestMethod]
        public void QuandoInstancioDoisServicosNaoRetornaMesmaInstancia()
        {
            var servico1 = ObjectFactory.GetInstance<ICadastroUsuario>();
            var servico2 = ObjectFactory.GetInstance<ICadastroUsuario>();
            Assert.AreNotSame(servico1, servico2);
        }


        [TestMethod]
        public void ConsigoMeConectarNoBancoDeDados()
        {
            var unitOfWorkNh = ObjectFactory.GetInstance<IUnitOfWorkNh>();
            Assert.AreEqual(ConnectionState.Open, unitOfWorkNh.Session.Connection.State);
        }

    }
}
