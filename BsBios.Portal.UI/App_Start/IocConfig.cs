using System.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.Infra.Factory;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.IoC;
using BsBios.Portal.UI.App_Start;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.UI
{
    public class IocConfig
    {
        public static void RegisterIoc()
        {
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBios"].ConnectionString);

            var emailDoPortal = ConfigurationManager.GetSection("emailDoPortal") as EmailDoPortal;
            var credencialSap = ConfigurationManager.GetSection("credencialSap") as CredencialSap;

            ObjectFactory.Configure(x =>
            {
                if (emailDoPortal != null)
                {
                    x.For<ContaDeEmail>()
                     .Singleton()
                     .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteLogistica + ">", emailDoPortal.Dominio,
                                           emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                           emailDoPortal.Porta,emailDoPortal.HabilitarSsl)).Named(Constantes.ContaDeEmailDaLogistica);

                    x.For<ContaDeEmail>()
                     .Singleton()
                     .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteSuprimentos + ">", emailDoPortal.Dominio,
                                           emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                           emailDoPortal.Porta,emailDoPortal.HabilitarSsl)).Named(Constantes.ContaDeEmailDeSuprimentos);
                }
                if (credencialSap != null)
                {
                    x.For<CredencialSap>()
                     .Singleton()
                     .Use(credencialSap);
                }
            });



            IoCWorker.Configure();

            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry<ControllerRegistry>();
                    x.For<IControllerFactory>()
                     .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                     .Use<StructureMapControllerFactory>();
                });
            ControllerBuilder.Current.SetControllerFactory(ObjectFactory.GetInstance<IControllerFactory>());

            var container = ObjectFactory.Container;
            GlobalConfiguration.Configuration.DependencyResolver = new StructureMapDependencyResolver(container);
        }
    }
}