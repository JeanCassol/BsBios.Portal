using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class RepositoryRegistry: Registry
    {
        public RepositoryRegistry()
        {
            For<IUsuarios>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Usuarios>();
            For<IProdutos>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Produtos>();
        }
    }
}