using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.IoC
{
    public class DomainServiceRegistry:  Registry
    {
        public DomainServiceRegistry()
        {
            For<ISelecionaFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<SelecionaFornecedor>();
        }
    }
}
