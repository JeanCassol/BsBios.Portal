using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class IncotermMap:ClassMap<Incoterm>
    {
        public IncotermMap()
        {
            Table("Incoterm");
            Id(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}
