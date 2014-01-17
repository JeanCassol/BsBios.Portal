using System.Collections.Generic;
using System.Configuration;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.IoC;
using BsBios.Portal.UI.App_Start;
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
            var credencialSap = ConfigurationManager.GetSection("credencialSap") as CredencialSap;

            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry<ControllerRegistry>();
                    if (emailDoPortal != null)
                    {
                        x.For<ContaDeEmail>()
                         .Singleton()
                         .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteLogistica + ">", emailDoPortal.Dominio,
                                               emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                               emailDoPortal.Porta, emailDoPortal.HabilitarSsl)).Named(Constantes.ContaDeEmailDaLogistica);

                        x.For<ContaDeEmail>()
                         .Singleton()
                         .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteSuprimentos + ">", emailDoPortal.Dominio,
                                               emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                               emailDoPortal.Porta, emailDoPortal.HabilitarSsl)).Named(Constantes.ContaDeEmailDeSuprimentos);
                    }
                    if (credencialSap != null)
                    {
                        x.For<CredencialSap>()
                         .Singleton()
                         .Use(credencialSap);
                    }

                });

        }
    }
}
