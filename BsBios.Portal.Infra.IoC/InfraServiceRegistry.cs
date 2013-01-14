using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.ApplicationServices.Implementation;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.Infra.IoC
{
    public class InfraServiceRegistry : Registry
    {
        public InfraServiceRegistry()
        {
            For<IAuthenticationProvider>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AuthenticationProvider>();
        }
    }
}