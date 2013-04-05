using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.UI.Controllers;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.UI.App_Start
{
    public class ControllerRegistry:  Registry
    {
        public ControllerRegistry()
        {
            For<ProcessoDeCotacaoDeMaterialAberturaController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeMaterialAberturaController>()
             .Ctor<IAberturaDeProcessoDeCotacaoServiceFactory>()
             .Is<AberturaDeProcessoDeCotacaoDeMaterialServiceFactory>();

            For<ProcessoDeCotacaoDeFreteAberturaController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeFreteAberturaController>()
             .Ctor<IAberturaDeProcessoDeCotacaoServiceFactory>()
             .Is<AberturaDeProcessoDeCotacaoDeFreteServiceFactory>();

            For<ProcessoDeCotacaoDeMaterialFechamentoController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeMaterialFechamentoController>()
             .Ctor<IFechamentoDeProcessoDeCotacaoServiceFactory>()
             .Is<FechamentoDeProcessoDeCotacaoDeMaterialServiceFactory>();

            For<ProcessoDeCotacaoDeFreteFechamentoController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeFreteFechamentoController>()
             .Ctor<IFechamentoDeProcessoDeCotacaoServiceFactory>()
             .Is<FechamentoDeProcessoDeCotacaoDeFreteServiceFactory>();

            For<ProcessoDeCotacaoDeFreteEmailController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeFreteEmailController>()
             .Ctor<IReenviadorDeEmailDoProcessoDeCotacaoFactory>()
             .Is<ReenviadorDeEmailDoProcessoDeCotacaoDeFreteFactory>();

            For<ProcessoDeCotacaoDeMaterialEmailController>()
             .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
             .Use<ProcessoDeCotacaoDeMaterialEmailController>()
             .Ctor<IReenviadorDeEmailDoProcessoDeCotacaoFactory>()
             .Is<ReenviadorDeEmailDoProcessoDeCotacaoDeMaterialFactory>();

        }
    }
}
