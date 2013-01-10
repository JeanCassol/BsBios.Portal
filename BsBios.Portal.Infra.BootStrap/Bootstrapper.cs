using System.Collections.Generic;
using BsBios.Portal.Infra.IoC;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.BootStrap
{
    public static class Bootstrapper
    {
        //static Bootstrapper()
        //{
        //    ConfigureContainer();
        //}

        public static void Run()
        {
            ConfigureContainer();
            IList<IBootstrapperTask> tasks = ObjectFactory.GetAllInstances<IBootstrapperTask>();

            foreach (IBootstrapperTask task in tasks)
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

                                            x.For<IBootstrapperTask>()
                                                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest)).
                                                Add<RegisterControllerFactory> ();


                                            x.For<IBootstrapperTask>()
                                                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest)).
                                                Add<RegisterRoutes>();

                                            x.For<IBootstrapperTask>()
                                             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                                             .Add<ConfigureDataAccess>();

                                        });
        }
    }
}