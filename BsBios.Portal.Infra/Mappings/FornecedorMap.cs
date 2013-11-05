using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class FornecedorMap: ClassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            Id(x => x.Codigo).GeneratedBy.Assigned();
            Map(x => x.Nome);
            Map(x => x.Email);
            Map(x => x.Cnpj);
            Map(x => x.Municipio);
            Map(x => x.Uf);
            Map(x => x.Endereco);
            Map(x => x.Transportadora);
            HasManyToMany(x => x.Produtos)
                .Cascade.All()
                .Table("ProdutoFornecedor").ParentKeyColumn("CodigoFornecedor").ChildKeyColumn("CodigoProduto")
                //.Fetch.Join() /*Removido o Fetch.Join() porque esta sempre fazendo join com ProdutoFornecedor e depois com Produtos*/
                .ExtraLazyLoad();

            
        }
    }
}
