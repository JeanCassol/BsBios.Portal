using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class IvaMap:ClassMap<Iva>
    {
        public IvaMap()
        {
            Table("IVA");
            Id(x => x.Id).GeneratedBy.Sequence("IVA_ID_SEQUENCE");
            Map(x => x.CodigoSap);
            Map(x => x.Descricao);
        }
    }
}
