using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class HistoricoDePrecoMap: ClassMap<HistoricoDePreco>
    {
        public HistoricoDePrecoMap()
        {
            Table("HistoricoDePreco");
            Id(x => x.Id).GeneratedBy.Sequence("HISTORICODEPRECO_ID_SEQUENCE");
            Map(x => x.DataHora);
            Map(x => x.Valor);
        }
    }
}