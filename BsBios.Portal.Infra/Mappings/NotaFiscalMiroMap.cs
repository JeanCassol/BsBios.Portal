using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class NotaFiscalMiroMap: ClassMap<NotaFiscalMiro>
    {
        public NotaFiscalMiroMap()
        {
            Table("NOTAFISCALMIRO");

            CompositeId()
                .KeyProperty(x => x.CnpjDoFornecedor)
                .KeyProperty(x => x.Numero)
                .KeyProperty(x => x.Serie);

            Map(x => x.MensagemDeProcessamento);

        }
    }
}
