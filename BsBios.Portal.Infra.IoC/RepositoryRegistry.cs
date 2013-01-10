using StructureMap.Configuration.DSL;

namespace BsBios.Portal.Infra.IoC
{
    public class RepositoryRegistry: Registry
    {
        public RepositoryRegistry()
        {
            //For<ICorretoras>()
            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
            //    .Use<Corretoras>();

        }
    }
}