using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Repositories.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class RepositoryRegistry: Registry
    {
        public RepositoryRegistry()
        {
            For<IUsuarios>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Usuarios>();
            For<IProdutos>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Produtos>();
            For<IProdutosDeFrete>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProdutosDeFrete>()
                .Ctor<string>("tiposDeProdutoDeFrete")
                .EqualToAppSetting("TiposDeProdutoDeFrete");

            For<IFornecedores>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Fornecedores>();

            For<ICondicoesDePagamento>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CondicoesDePagamento>();
            For<IIvas>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Ivas>();
            For<IIncoterms>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Incoterms>();
            For<IRequisicoesDeCompra>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<RequisicoesDeCompra>();
            For<IProcessosDeCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessosDeCotacao>();
            For<IProcessosDeCotacaoDeFrete>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessosDeCotacaoDeFrete>();
            For<IItinerarios>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Itinerarios>();
            For<IUnidadesDeMedida>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<UnidadesDeMedida>();

            For<IQuotas>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Quotas>();

            For<IProcessoCotacaoIteracoesUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoCotacaoIteracoesUsuario>();

            For<IMunicipios>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Municipios>();

            For<IOrdensDeTransporte>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<OrdensDeTransporte>();

            For<ITerminais>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<Terminais>();

            For<IMateriaisDeCarga>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<MateriaisDeCarga>();

        }
    }
}