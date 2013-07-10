using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class CotacaoItemMap: ClassMap<CotacaoItem>
    {
        public CotacaoItemMap()
        {
            Table("CotacaoItem");
            /*Removi o composite Id porque não conseguia mapear as subclasses. Dava o erro
             NHibernate.FKUnmatchingColumnsException: Foreign key (FKD37B35BECAD2B9E0:CotacaoFreteItem [IdCotacao])) must have same number of columns as the referenced primary key (CotacaoItem [IdCotacao, IdProcessoCotacaoItem]*/
            //CompositeId()
            //    .KeyReference(x => x.Cotacao, "IdCotacao")
            //    .KeyReference(x => x.ProcessoDeCotacaoItem, "IdProcessoCotacaoItem");

            Id(x => x.Id).GeneratedBy.Sequence("COTACAOITEM_ID_SEQUENCE");

            References(x => x.Cotacao, "IdCotacao");
            References(x => x.ProcessoDeCotacaoItem, "IdProcessoCotacaoItem");

            Map(x => x.QuantidadeAdquirida);
            Map(x => x.Preco);
            Map(x => x.PrecoInicial);
            Map(x => x.ValorComImpostos);
            Map(x => x.Custo);
            Map(x => x.Selecionada);
            Map(x => x.QuantidadeDisponivel);
            Map(x => x.Observacoes);

            /*Importante: Sem a opção "Cascade.AllDeleteOrphan()" o NHibernate tenta sempre fazer um update mesmo quando é criado um imposto novo.
             * Lembrar de usar esta opção sempre que mapear com "HasMany"*/
            HasMany(x => x.Impostos)
                .KeyColumn("IdCotacaoItem")
                .Inverse()
                .Cascade.AllDeleteOrphan();

            //Para fazer relacionamento unidirecional sem que o NHibernate faça update adicionais desnecessários 
            //(INSERT e depois UPDATE na chave estrangeira (IdCotacaoItem)) é necessário que as seguintes configurações
            //sejam feitas:
                //.Not.Inverse()
                //.Not.KeyNullable()
                //.Not.KeyUpdate()
            HasMany(x => x.HistoricosDePreco)
                .KeyColumn("IdCotacaoItem")
                .Not.Inverse()
                .Not.KeyNullable()
                .Not.KeyUpdate()
                .Cascade.All();
        }
    }

    public class CotacaoMaterialItemMap: SubclassMap<CotacaoMaterialItem>
    {
        public CotacaoMaterialItemMap()
        {
            Table("CotacaoMaterialItem");
            //KeyColumn("IdCotacao");
            //KeyColumn("IdProcessoCotacaoItem");
            KeyColumn("Id");

            References(x => x.Iva).Column("CodigoIva");
            Map(x => x.Mva);
            Map(x => x.PrazoDeEntrega).Column("PrazoEntrega");
        }
    }
    public class CotacaoFreteItemMap: SubclassMap<CotacaoFreteItem>
    {
        public CotacaoFreteItemMap()
        {
            Table("CotacaoFreteItem");
            //KeyColumn("IdCotacao");
            //KeyColumn("IdProcessoCotacaoItem");
            KeyColumn("Id");
        }
    }
}
