using System.Collections.Generic;
using System.Web.Mvc;
using BsBios.Portal.Infra.Factory;
using BsBios.Portal.Infra.IoC;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.BootStrap
{
    public static class Bootstrapper
    {
        public static void Run()
        {
            ConfigureContainer();
            IList<IBootstrapTask> tasks = ObjectFactory.GetAllInstances<IBootstrapTask>();

            foreach (IBootstrapTask task in tasks)
            {
                task.Execute();
            }
        }

        private static void ConfigureContainer()
        {
            IoCWorker.Configure();

            ObjectFactory.Configure(x =>
                                        {
                                            x.For<IControllerFactory>()
                                                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                                                .Use<StructureMapControllerFactory>();

                                            //x.For<IAutenticador>()
                                            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                                            //    .Use<Autenticador>();

                                            x.For<IBootstrapTask>()
                                                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest)).
                                                Add<RegisterControllerFactory> ();


                                            x.For<IBootstrapTask>()
                                                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest)).
                                                Add<RegisterRoutes>();

                                            //x.For<IBootstrapTask>()
                                            // .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                                            // .Add<ConfigureDataAccess>();

                                        });
        }
    }
}