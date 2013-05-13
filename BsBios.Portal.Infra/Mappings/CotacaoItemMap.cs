using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;
using FluentNHibernate.MappingModel;

namespace BsBios.Portal.Infra.Mappings
{
    public class CotacaoItemMap: ClassMap<CotacaoItem>
    {
        public CotacaoItemMap()
        {
            Table("CotacaoItem");
            CompositeId()
                .KeyReference(x => x.Cotacao, "IdCotacao")
                .KeyReference(x => x.ProcessoDeCotacaoItem, "IdProcessoCotacaoItem");

            Map(x => x.QuantidadeAdquirida);
            Map(x => x.ValorLiquido);
            Map(x => x.ValorComImpostos);
            Map(x => x.Selecionada);
            Map(x => x.QuantidadeDisponivel);
            Map(x => x.Observacoes);

            /*Importante: Sem a opção "Cascade.AllDeleteOrphan()" o NHibernate tenta sempre fazer um update mesmo quando é criado um imposto novo.
             * Lembrar de usar esta opção sempre que mapear com "HasMany"*/
            HasMany(x => x.Impostos)
                .KeyColumn("IdCotacao")
                .KeyColumn("IdProcessoCotacaoItem")
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }

    public class CotacaoItemMaterialMap: SubclassMap<CotacaoItemMaterial>
    {
        public CotacaoItemMaterialMap()
        {
            Table("CotacaoItemMaterial");
            KeyColumn("IdProcessoCotacao");
            KeyColumn("IdProcessoCotacaoItem");

            References(x => x.Iva).Column("CodigoIva");
            Map(x => x.Mva);
            Map(x => x.PrazoDeEntrega).Column("PrazoEntrega");
        }
    }
    public class CotacaoItemFreteMap: SubclassMap<CotacaoItemFrete>
    {
        public CotacaoItemFreteMap()
        {
            Table("CotacaoItemFrete");
            KeyColumn("IdProcessoCotacao");
            KeyColumn("IdProcessoCotacaoItem");
        }
    }
}
