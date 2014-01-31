using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class TerminalMap: ClassMap<Terminal>
    {
        public TerminalMap()
        {
            Table("TERMINAL");
            Id(x => x.Codigo);
            Map(x => x.Nome);
            Map(x => x.Cidade);
        }
    }
}
