using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ColetaMap: ClassMap<Coleta>
    {
        public ColetaMap()
        {
            Table("Coleta");
            Id(x => x.Id).GeneratedBy.Sequence("COLETA_ID_SEQUENCE");

            Map(x => x.Realizado);
            Map(x => x.DataDePrevisaoDeChegada);
            Map(x => x.Placa);
            Map(x => x.Motorista);
            Map(x => x.Peso);
            Map(x => x.ValorDoFrete);

            HasMany(x => x.NotasFiscais)
                            .KeyColumn("IdColeta")
                            .Not.Inverse()
                            .Not.KeyNullable()
                            .Not.KeyUpdate()
                            .Cascade.AllDeleteOrphan();
        }
    }
}
