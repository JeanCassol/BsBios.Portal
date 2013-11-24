using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class NotaFiscalDeColetaMap: ClassMap<NotaFiscalDeColeta>
    {
        public NotaFiscalDeColetaMap()
        {
            Table("NOTAFISCALDECOLETA");
            Id(x => x.Id).GeneratedBy.Sequence("NOTAFISCALDECOLETA_ID_SEQUENCE");
            Map(x => x.Numero);
            Map(x => x.NumeroDoConhecimento);
            Map(x => x.Serie);
            Map(x => x.DataDeEmissao,"DataEmissao");
            Map(x => x.Peso);
            Map(x => x.Valor);
        }
    }
}
