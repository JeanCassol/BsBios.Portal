using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class DomainServiceRegistry:  Registry
    {
        public DomainServiceRegistry()
        {
            For<IVerificaPermissaoAgendamento>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<VerificaPermissaoAgendamento>();

            For<IProcessoDeCotacaoDeFreteFactory>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeFreteFactory>();

            For<IProcessoDeCotacaoDeMaterialFactory>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeMaterialFactory>();
        }
    }
}
