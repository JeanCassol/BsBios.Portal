using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class MaterialDeCargaMap: ClassMap<MaterialDeCarga>
    {
        public MaterialDeCargaMap()
        {
            Table("MaterialDeCarga");
            Id(x => x.Codigo).GeneratedBy.Assigned();
            Map(x => x.Descricao);

        }
    }
}
