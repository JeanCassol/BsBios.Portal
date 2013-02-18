using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class IvaMap:ClassMap<Iva>
    {
        public IvaMap()
        {
            Table("IVA");
            Id(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}
