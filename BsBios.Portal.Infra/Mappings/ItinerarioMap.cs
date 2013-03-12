using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ItinerarioMap:ClassMap<Itinerario>
    {
        public ItinerarioMap()
        {
            Table("ITINERARIO");
            Id(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}
