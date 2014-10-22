using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ConhecimentoDeTransporteMap: ClassMap<ConhecimentoDeTransporte>
    {
        public ConhecimentoDeTransporteMap()
        {
            Table("ConhecimentoDeTransporte");

            Id(x => x.ChaveEletronica).GeneratedBy.Assigned();

            Map(x => x.CnpjDoFornecedor);
            Map(x => x.CnpjDaTransportadora);
            Map(x => x.DataDeEmissao);
            Map(x => x.Numero);
            Map(x => x.Serie);
            Map(x => x.NumeroDoContrato);
            Map(x => x.ValorRealDoFrete);
            Map(x => x.PesoTotalDaCarga);
            Map(x => x.Status).CustomType<Enumeradores.StatusDoConhecimentoDeTransporte>();
            Map(x => x.MensagemDeErroDeProcessamento);

            References(x => x.Fornecedor).Column("CodigoFornecedor");
            References(x => x.Transportadora).Column("CodigoTransportadora");

            HasMany(ConhecimentoDeTransporte.Expressions.NotasFiscais)
                            .KeyColumn("CHAVECONHECIMENTODETRANSPORTE")
                            .Not.Inverse()
                            .Not.KeyNullable()
                            .Not.KeyUpdate()
                            .Cascade.AllDeleteOrphan()
                            .ExtraLazyLoad();

            HasManyToMany(ConhecimentoDeTransporte.Expressions.OrdensDeTransporteVinculadas)
                            .Table("CONHECIMENTO_ORDEMTRANSPORTE")
                            .ParentKeyColumn("ChaveEletronica")
                            .ChildKeyColumn("IdOrdemTransporte")
                            .Cascade.All()
                            .ExtraLazyLoad();

        }

    }
}

