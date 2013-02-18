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
            Id(x => x.Codigo);
            //Map(x => x.Codigo);
            Map(x => x.Descricao);
        }
    }
}
