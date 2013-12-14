using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Queries.Implementations;
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
            For<IConsultaIncoterm>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaIncoterm>();
            For<IConsultaIva>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaIva>();

            For<IConsultaProcessoDeCotacaoDeMaterial>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaProcessoDeCotacaoDeMaterial>();
            For<IConsultaFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaFornecedor>();
            For<IConsultaProduto>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaProduto>();
            For<IConsultaCotacaoDoFornecedor>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaCotacaoDoFornecedor>();

            For<IConsultaUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaUsuario>();

            For<IConsultaPerfil>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaPerfil>();

            For<IConsultaFluxoDeCarga>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaFluxoDeCarga>();

            For<IConsultaUnidadeDeMedida>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaUnidadeDeMedida>();

            For<IConsultaItinerario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaItinerario>();

            For<IConsultaProcessoDeCotacaoDeFrete>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaProcessoDeCotacaoDeFrete>();

            For<IConsultaQuota>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaQuota>();

            For<IConsultaFluxoDeCarga>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaFluxoDeCarga>();

            For<IConsultaMaterialDeCarga>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaMaterialDeCarga>();

            For<IConsultaRealizacaoDeAgendamento>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaRealizacaoDeAgendamento>();

            For<IConsultaStatusProcessoCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaStatusProcessoCotacao>();

            For<IConsultaQuotaRelatorio>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaQuotaRelatorio>();

            For<IConsultaMunicipio>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaMunicipio>();

            For<IConsultaOrdemDeTransporte>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaOrdemDeTransporte>();

            For<IConsultaParaConferenciaDeCargas>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaParaConferenciaDeCargas>();

            For<IConsultaMonitorDeOrdemDeTransporte>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaMonitorDeOrdemDeTransporte>();

            For<IConsultaSelecaoDeFornecedores>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaSelecaoDeFornecedores>();

            For<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaRelatorioDeProcessoDeCotacaoDeFrete>();

            For<IConsultaEscolhaSimples>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ConsultaEscolhaSimples>();

        }

    }
}