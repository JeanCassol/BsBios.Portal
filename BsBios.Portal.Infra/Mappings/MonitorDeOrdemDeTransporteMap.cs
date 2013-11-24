using BsBios.Portal.ViewModel;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class MonitorDeOrdemDeTransporteMap: ClassMap<MonitorDeOrdemDeTransporteVm>
    {
        public MonitorDeOrdemDeTransporteMap()
        {
            Id(x => x.Id,"ID");
            Map(x => x.Fornecedor);
            Map(x => x.Material);
            Map(x => x.QuantidadeLiberada);
            Map(x => x.QuantidadeEmTransito);
            Map(x => x.QuantidadePendente);
            Map(x => x.PrevisaoDeChegadaNoDia);
        }
    }
}
