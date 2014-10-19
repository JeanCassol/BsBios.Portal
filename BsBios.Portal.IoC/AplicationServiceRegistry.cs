using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
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
            For<ICadastroFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroFornecedor>();
            For<ICadastroProdutoFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroProdutoFornecedor>();
            For<ICadastroCondicaoPagamento>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroCondicaoPagamento>();
            For<ICadastroIva>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroIva>();
            For<ICadastroIncoterm>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroIncoterm>();
            For<ICadastroRequisicaoCompra>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroRequisicaoCompra>();

            For<ICadastroItinerario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroItinerario>();

            For<ICadastroUnidadeDeMedida>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroUnidadeDeMedida>();

            For<IProcessoDeCotacaoDeMaterialService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeMaterialService>();
            For<IProcessoDeCotacaoFornecedoresService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoFornecedoresService>();

            For<IProcessoDeCotacaoDeMaterialSelecaoService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeMaterialSelecaoService>();

            For<IProcessoDeCotacaoDeFreteSelecaoService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeFreteSelecaoService>();

            For<IAtualizadorDeCotacaoDeMaterial>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AtualizadorDeCotacaoDeMaterial>();

            For<IAtualizadorDeCotacaoDeFrete>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AtualizadorDeCotacaoDeFrete>();

            For<IGerenciadorUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GerenciadorUsuario>();

            For<IProcessoDeCotacaoDeFreteService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProcessoDeCotacaoDeFreteService>();

            For<ICadastroQuota>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroQuota>();

            For<IAgendamentoDeCargaService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AgendamentoDeCargaService>();

            For<IAberturaDeProcessoDeCotacaoService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AberturaDeProcessoDeCotacaoService>();

            For<IFechamentoDeProcessoDeCotacaoService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<FechamentoDeProcessoDeCotacaoDeMaterialService>();

            For<ICancelamentoDeProcessoDeCotacaoService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CancelamentoDeProcessoDeCotacaoService>();

            For<IReenviadorDeEmailDoProcessoDeCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ReenviadorDeEmailDoProcessoDeCotacao>();

            For<IOrdemDeTransporteService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<OrdemDeTransporteService>();

            For<ICadastroTerminal>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroTerminal>();

            For<ICadastroDeConhecimentoDeTransporte>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroDeConhecimentoDeTransporte>();

            For<ICadastroDeNotaFiscalMiro>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<CadastroDeNotaFiscalMiro>();

        }
    }
}