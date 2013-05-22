using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class RequisicaoDeCompraMap: ClassMap<RequisicaoDeCompra>
    {
        public RequisicaoDeCompraMap()
        {
            Table("RequisicaoCompra");
            Id(x => x.Id).GeneratedBy.Sequence("REQUISICAOCOMPRA_ID_SEQUENCE");

            References(x => x.Material).Column("CodigoMaterial");
            References(x => x.FornecedorPretendido).Column("CodigoFornecedorPretendido");
            References(x => x.Criador).Column("LoginCriador");
            References(x => x.UnidadeMedida).Column("UnidadeMedida");

            Map(x => x.Centro);
            Map(x => x.Requisitante);
            Map(x => x.DataDeLiberacao).Column("DataLiberacao");
            Map(x => x.DataDeRemessa).Column("DataRemessa");
            Map(x => x.DataDeSolicitacao).Column("DataSolicitacao");
            Map(x => x.Numero);
            Map(x => x.NumeroItem);
            Map(x => x.Descricao);
            Map(x => x.Quantidade);
            Map(x => x.CodigoGrupoDeCompra);
            Map(x => x.Mrp);
            Map(x => x.GerouProcessoDeCotacao);
        }
    }
}
