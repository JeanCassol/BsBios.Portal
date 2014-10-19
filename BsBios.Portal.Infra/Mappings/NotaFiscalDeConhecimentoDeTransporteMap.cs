using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class NotaFiscalDeConhecimentoDeTransporteMap: ClassMap<NotaFiscalDeConhecimentoDeTransporte>
    {
        public NotaFiscalDeConhecimentoDeTransporteMap()
        {
            Table("NFCONHECIMENTOTRANSPORTE");
            Id(x => x.ChaveEletronica);

            Map(x => x.Numero);
            Map(x => x.Serie);
        }
    }
}
