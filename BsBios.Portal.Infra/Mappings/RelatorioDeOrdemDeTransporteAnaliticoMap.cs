using BsBios.Portal.ViewModel;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class RelatorioDeOrdemDeTransporteAnaliticoMap: ClassMap<RelatorioDeOrdemDeTransporteAnaliticoVm>
    {
        public RelatorioDeOrdemDeTransporteAnaliticoMap()
        {
            Id(x => x.Id);
            Map(x => x.IdDaOrdemDeTransporte);
            //Map(x => x.Status);
            Map(x => x.Material);
            Map(x => x.UnidadeDeMedida);
            Map(x => x.DataDeValidadeInicial);
            Map(x => x.DataDeValidadeFinal);
            Map(x => x.Itinerario);
            Map(x => x.NomeDoFornecedorDaMercadoria);
            Map(x => x.NomeDoDeposito);
            Map(x => x.Transportadora);
            Map(x => x.MunicipioDeOrigem);
            Map(x => x.MunicipioDeDestino);
            Map(x => x.Classificacao);
            Map(x => x.Cadencia);
            Map(x => x.QuantidadeContratada);
            Map(x => x.QuantidadeLiberada);
            Map(x => x.QuantidadeDeTolerancia);
            Map(x => x.QuantidadeEmTransito);
            Map(x => x.QuantidadeRealizada);
            Map(x => x.QuantidadePendente);
            Map(x => x.QuantidadeDeColetasRealizadas);
            Map(x => x.QuantidadeDeDiasEmAtraso);
            Map(x => x.PercentualDeAtraso);
            Map(x => x.MotivoDeFechamento);
            Map(x => x.ObservacaoDeFechamento);
        }

    }
}
