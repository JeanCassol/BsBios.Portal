using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class FornecedorMap: ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            Id(x => x.Codigo);
            Map(x => x.Nome);
            Map(x => x.Email);
            HasManyToMany(x => x.Produtos)
                .Cascade.All()
                .Table("ProdutoFornecedor").ParentKeyColumn("CodigoFornecedor").ChildKeyColumn("CodigoProduto");

        }
    }
}
