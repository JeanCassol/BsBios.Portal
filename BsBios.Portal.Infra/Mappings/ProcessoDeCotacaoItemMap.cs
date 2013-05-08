using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProcessoDeCotacaoItemMap: ClassMap<ProcessoDeCotacaoItem>
    {
        public ProcessoDeCotacaoItemMap()
        {
            Table("ProcessoCotacaoItem");
            Id(x => x.Id).GeneratedBy.Sequence("PROCESSOCOTACAOITEM_ID_SEQUENCE");
            References(x => x.Produto).Column("CodigoProduto");
            References(x => x.UnidadeDeMedida).Column("CodigoUnidadeMedida");
            Map(x => x.Quantidade);
        }
    }
    public class ProcessoDeCotacaoDeMaterialItemMap:SubclassMap<ProcessoDeCotacaoDeMaterialItem>
    {
        public ProcessoDeCotacaoDeMaterialItemMap()
        {
            Table("ProcessoCotacaoMaterialItem");
            KeyColumn("Id");
            References(x => x.RequisicaoDeCompra).Column("IdRequisicaoCompra");
        }
    }
    public class ProcessoDeCotacaoDeFreteItem: SubclassMap<ProcessoDeCotacaoDeFreteItem>
    {
        public ProcessoDeCotacaoDeFreteItem()
        {
            Table("ProcessoCotacaoFreteItem");
            KeyColumn("Id");
        }
    }
}
