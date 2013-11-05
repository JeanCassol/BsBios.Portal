using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Conventions;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class OrdemDeTransporteMap: ClassMap<OrdemDeTransporte>
    {
        public OrdemDeTransporteMap()
        {
            Table("ORDEMDETRANSPORTE");
            Id(x => x.Id).GeneratedBy.Sequence("ORDEMDETRANSPORTE_ID_SEQUENCE");
            
            References(x => x.Fornecedor).Column("CODIGOFORNECEDOR").Not.Nullable();
            References(x => x.ProcessoDeCotacaoDeFrete).Column("IDPROCESSOCOTACAOFRETE").Not.Nullable();

            Map(x => x.QuantidadeAdquirida);
            Map(x => x.QuantidadeLiberada);
            Map(x => x.QuantidadeColetada);
            Map(x => x.PrecoUnitario);

        }
    }
}
