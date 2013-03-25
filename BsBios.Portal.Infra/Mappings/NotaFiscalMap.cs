using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class NotaFiscalMap: ClassMap<NotaFiscal>
    {
        public NotaFiscalMap()
        {
            Table("NotaFiscal");
            CompositeId()
                .KeyProperty(x => x.Numero)
                .KeyProperty(x => x.Serie)
                .KeyProperty(x => x.CnpjDoEmitente,"CnpjEmitente");

            References(x => x.AgendamentoDeDescarregamento).Column("IdAgendamentoDescarregamento");

            Map(x => x.DataDeEmissao, "DataEmissao");
            Map(x => x.CnpjDoContratante, "CnpjContratante");
            Map(x => x.NomeDoContratante, "NomeContratante");
            Map(x => x.NomeDoEmitente, "NomeEmitente");
            Map(x => x.NumeroDoContrato, "NumeroContrato");
            Map(x => x.Peso);
            Map(x => x.Valor);

        }
    }
}
