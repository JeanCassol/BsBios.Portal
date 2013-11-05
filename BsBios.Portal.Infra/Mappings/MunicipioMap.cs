using BsBios.Portal.Domain.ValueObjects;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class MunicipioMap: ClassMap<Municipio>
    {
        public MunicipioMap()
        {
            ReadOnly();
            Table("MUNICIPIO");
            Id(x => x.Codigo).GeneratedBy.Assigned();
            Map(x => x.Nome);
            Map(x => x.UF);
        }
    }
}
