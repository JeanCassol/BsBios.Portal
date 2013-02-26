using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProcessoDeCotacaoMap: ClassMap<ProcessoDeCotacao>
    {
        public ProcessoDeCotacaoMap()
        {
            Table("ProcessoCotacao");
            Id(x => x.Id).GeneratedBy.Sequence("PROCESSOCOTACAO_ID_SEQUENCE");
            References(x => x.Produto).Column("CodigoProduto");
            Map(x => x.Status).CustomType<Enumeradores.StatusProcessoCotacao>();
            Map(x => x.Quantidade);
            Map(x => x.DataLimiteDeRetorno).Column("DataLimiteRetorno");

            //Não usar DiscriminateSubClassesOnColumn porque deve ser utilizado apenas quando a estratégia é uma tabela para toda a hierarquia
            //DiscriminateSubClassesOnColumn("TipoCotacao").CustomType<Enumeradores.TipoDeCotacao>();

            //Não funciona porque não faz junção com a tabela referente à classe abstrata. Procura todos os campos na tabela referente à classe concreta
            //UseUnionSubclassForInheritanceMapping();
        }
    }

    public class ProcessoDeCotacaoDeMaterialMap : SubclassMap<ProcessoDeCotacaoDeMaterial>
    {
        public ProcessoDeCotacaoDeMaterialMap()
        {
            Table("ProcessoCotacaoMaterial");
            KeyColumn("Id");
            References(x => x.RequisicaoDeCompra).Column("IdRequisicaoCompra");
            DiscriminatorValue(Enumeradores.TipoDeCotacao.Material);
        }
    }
}
