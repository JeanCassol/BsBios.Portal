using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
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
            For<IAccountService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AccountService>();
            For<IProvedorDeCriptografia>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProvedorDeCriptografiaMd5>();
            For<IEmailService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<EmailService>();
            For<IGeradorDeSenha>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeSenha>();
        }
    }
}