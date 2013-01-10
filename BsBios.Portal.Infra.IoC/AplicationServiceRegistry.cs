using StructureMap.Configuration.DSL;

namespace BsBios.Portal.Infra.IoC
{
    public class AplicationServiceRegistry : Registry
    {
        public  AplicationServiceRegistry()
        {
            //For<IHelloWorld>()
            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
            //    .Use<HelloWorld>();
            //For<ISendMessage>()
            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
            //    .Use<SendMessage>();

        }
    }
}