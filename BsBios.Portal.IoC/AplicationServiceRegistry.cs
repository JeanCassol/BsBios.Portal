using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.ApplicationServices.Implementation;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class AplicationServiceRegistry : Registry
    {
        public  AplicationServiceRegistry()
        {
            For<ICadastroUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroUsuario>();
            For<ICadastroProduto>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroProduto>();
        }
    }
}