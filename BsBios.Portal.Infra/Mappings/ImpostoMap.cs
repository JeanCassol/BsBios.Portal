using BsBios.Portal.Common;
using BsBios.Portal.Domain;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ImpostoMap: ClassMap<Imposto>
    {
        public ImpostoMap()
        {
            Table("CotacaoImposto");
            CompositeId()
                .KeyReference(x => x.CotacaoItem, "IdCotacao")
                .KeyProperty(x => x.Tipo, "TipoImposto").CustomType<Enumeradores.TipoDeImposto>();
            Map(x => x.Aliquota);
            Map(x => x.Valor);

            //References(x => x.Cotacao, "IdCotacao").ForeignKey();

        }
    }
}
