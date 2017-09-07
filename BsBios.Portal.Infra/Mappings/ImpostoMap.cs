using BsBios.Portal.Common;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ImpostoMap: ClassMap<Imposto>
    {
        public ImpostoMap()
        {
            Table("CotacaoItemImposto");
            CompositeId()
                .KeyReference(x => x.CotacaoItem, "IdCotacaoItem")
                .KeyProperty(x => x.Tipo, "TipoImposto").CustomType<Enumeradores.TipoDeImposto>();
            Map(x => x.Aliquota);
            Map(x => x.Valor);

            //References(x => x.Cotacao, "IdCotacao").ForeignKey();

        }
    }
}
