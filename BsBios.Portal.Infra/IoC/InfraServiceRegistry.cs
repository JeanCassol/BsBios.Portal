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
            For<IValidadorUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ValidadorUsuario>();
            For<IAuthenticationProvider>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AuthenticationProvider>();
        }
    }
}