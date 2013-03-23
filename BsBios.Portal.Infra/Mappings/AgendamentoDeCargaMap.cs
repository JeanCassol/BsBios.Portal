
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class AgendamentoDeCargaMap:ClassMap<AgendamentoDeCarga>
    {
        public AgendamentoDeCargaMap()
        {
            Table("AgendamentoDeCarga");
            Id(x => x.Id).GeneratedBy.Sequence("AGENDAMENTODECARGA_ID_SEQUENCE");
            Map(x => x.Data);
            Map(x => x.Material,"CodigoMaterial").CustomType<Enumeradores.MaterialDeCarga>();
            Map(x => x.Placa);
            Map(x => x.Realizado);
        }
    }

    public class AgendamentoDeCarregamentoMap: SubclassMap<AgendamentoDeCarregamento>
    {
        public AgendamentoDeCarregamentoMap()
        {
            Table("AgendamentoDeCarregamento");
            KeyColumn("Id");
            Map(x => x.Peso);
        }
    }
    public class AgendamentoDeDescarregamentoMap: SubclassMap<AgendamentoDeDescarregamento>
    {
        public AgendamentoDeDescarregamentoMap()
        {
            Table("AgendamentoDeDescarregamento");
            KeyColumn("Id");
            HasMany(x => x.NotasFiscais).KeyColumn("IdAgendamentoDescarregamento")
                .ExtraLazyLoad()
                .Inverse()
                .Cascade.AllDeleteOrphan();
        }
    }

}
