using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class BuildersRegistry : Registry
    {
        public BuildersRegistry()
        {
            For<IBuilder<Fornecedor, FornecedorCadastroVm>>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<FornecedorCadastroBuilder>();
        }
    }
}