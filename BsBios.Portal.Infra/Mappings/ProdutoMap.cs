using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;
using NHibernate.Mapping;
//using NHibernate.Mapping.ByCode;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProdutoMap: ClassMap<Produto>
    {
        public ProdutoMap()
        {
            Table("Produto");
            Id(x => x.Codigo).GeneratedBy.Assigned();
            Map(x => x.Descricao);
            Map(x => x.Tipo);
            HasManyToMany(x => x.Fornecedores)
                .Cascade.All()
                .Table("ProdutoFornecedor").ParentKeyColumn("CodigoProduto").ChildKeyColumn("CodigoFornecedor")
                .Fetch.Join()
                .ExtraLazyLoad();
        }
    }
}
