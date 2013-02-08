using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class CondicaoDePagamentoMap : ClassMap<CondicaoDePagamento>
    {
        public CondicaoDePagamentoMap()
        {
            Table("CONDICAOPAGAMENTO");
            Id(x => x.Id).GeneratedBy.Sequence("CONDICAOPAGAMENTO_ID_SEQUENCE");
            Map(x => x.CodigoSap);
            Map(x => x.Descricao);
        }
    }
}
