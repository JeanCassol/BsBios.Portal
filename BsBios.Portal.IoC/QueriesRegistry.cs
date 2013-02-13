using Application.Queries.Contracts;
using Application.Queries.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class QueriesRegistry : Registry
    {
        public QueriesRegistry()
        {
            For<IConsultaCondicaoPagamento>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaCondicaoPagamento>();
        }
    }
}