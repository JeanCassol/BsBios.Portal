using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class UnidadeDeMedidaMap:ClassMap<UnidadeDeMedida>
    {
        public UnidadeDeMedidaMap()
        {
            Table("UNIDADEMEDIDA");
            Id(x => x.CodigoInterno);
            Map(x => x.CodigoExterno);
            Map(x => x.Descricao);
        }
    }
}
