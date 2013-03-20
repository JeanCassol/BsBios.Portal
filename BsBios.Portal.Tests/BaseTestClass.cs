using System.Collections.Generic;
using System.Configuration;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.IoC;
using BsBios.Portal.UI.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests
{
    [TestClass]
    public class BaseTestClass
    {
        [AssemblyInitialize]
        public static void TesteInitialize(TestContext context)
        {
            IoCWorker.Configure();
            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(new UsuarioConectado("teste", "Usuário de Teste", new List<Enumeradores.Perfil>{Enumeradores.Perfil.CompradorSuprimentos})));

            var emailDoPortal = ConfigurationManager.GetSection("emailDoPortal") as EmailDoPortal;

            ObjectFactory.Configure(x =>
                {
                    if (emailDoPortal != null)
                        x.For<ContaDeEmail>()
                         .Singleton()
                         .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteSuprimentos + ">", emailDoPortal.Dominio,
                                               emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                               emailDoPortal.Porta));
                });

        }
    }
}
