using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProdutoMap: ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("Produto");
            Id(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}
