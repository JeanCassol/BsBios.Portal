using System.Configuration;
using System.Web.Mvc;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.Infra.Factory;
using BsBios.Portal.IoC;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.UI
{
    public class IocConfig
    {
        public static void RegisterIoc()
        {
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBiosTesteUnitario"].ConnectionString);
            IoCWorker.Configure();
            ObjectFactory.Configure(x => x.For<IControllerFactory>()
                                          .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                                          .Use<StructureMapControllerFactory>());
            ControllerBuilder.Current.SetControllerFactory(ObjectFactory.GetInstance<IControllerFactory>());
        }
    }
}