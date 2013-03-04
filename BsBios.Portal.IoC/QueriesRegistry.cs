using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
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
            For<IConsultaProcessoDeCotacaoDeMaterial>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaProcessoDeCotacaoDeMaterial>();
            For<IConsultaFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaFornecedor>();
            For<IConsultaProduto>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaProduto>();
        }

    }
}