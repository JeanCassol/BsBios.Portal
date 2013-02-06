using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class FornecedorMap: ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            Id(x => x.Id).GeneratedBy.Sequence("FORNECEDOR_ID_SEQUENCE");
            Map(x => x.CodigoSap);
            Map(x => x.Nome).Column("Nome");
        }
    }
}
