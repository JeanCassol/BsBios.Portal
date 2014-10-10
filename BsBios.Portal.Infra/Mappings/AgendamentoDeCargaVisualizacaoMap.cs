using BsBios.Portal.ViewModel;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class AgendamentoDeCargaVisualizacaoMap : ClassMap<ConferenciaDeCargaPesquisaResultadoVm>
    {
        public AgendamentoDeCargaVisualizacaoMap()
        {
            Table("AGENDAMENTODECARGAVISUALIZACAO");
            Id(x => x.Id);
            Map(x => x.CodigoTerminal);
            Map(x => x.DescricaoTerminal);
            Map(x => x.IdAgendamento);
            Map(x => x.IdQuota);
            Map(x => x.IdOrdemTransporte);
            Map(x => x.IdColeta);
            Map(x => x.CnpjEmitente);
            Map(x => x.NomeEmitente);
            Map(x => x.DataAgendamento);
            Map(x => x.CodigoFluxo);
            Map(x => x.DescricaoFluxo);
            Map(x => x.DescricaoMaterial);
            Map(x => x.NumeroNf);
            Map(x => x.Placa);
            Map(x => x.Realizado);
            Map(x => x.CodigoDeposito);
        }
    }
}
