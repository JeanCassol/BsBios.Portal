using BsBios.Portal.ViewModel;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class MonitorDeOrdemDeTransporteMap: ClassMap<MonitorDeOrdemDeTransporteListagemVm>
    {
        public MonitorDeOrdemDeTransporteMap()
        {
            Id(x => x.Id);
            Map(x => x.Terminal); 
            Map(x => x.Material);
            Map(x => x.NumeroDoContrato,"NumeroContrato");
            Map(x => x.FornecedorDaMercadoria);
            Map(x => x.Transportadora);
            Map(x => x.NumeroDaOrdemDeTransporte);
            Map(x => x.MunicipioDeOrigem);
            Map(x => x.MunicipioDeDestino);
            Map(x => x.QuantidadeLiberada);
            Map(x => x.QuantidadeRealizada);
            Map(x => x.PercentualPendente);
            Map(x => x.PercentualProjetado);
            Map(x => x.QuantidadeEmTransito);
            Map(x => x.PrevisaoDeChegadaNoDia);
            Map(x => x.QuantidadePendente);
        }
    }
}
