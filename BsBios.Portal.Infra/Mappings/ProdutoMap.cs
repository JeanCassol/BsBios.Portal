using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProdutoMap: ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("Produto");
            Id(x => x.Id).GeneratedBy.Sequence("PRODUTO_ID_SEQUENCE");
            Map(x => x.CodigoSap);
            Map(x => x.Descricao);
        }
    }
}
