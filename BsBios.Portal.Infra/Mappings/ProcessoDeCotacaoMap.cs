using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProcessoDeCotacaoMap: ClassMap<ProcessoDeCotacao>
    {
        public ProcessoDeCotacaoMap()
        {
            Table("ProcessoCotacao");
            Id(x => x.Id).GeneratedBy.Sequence("PROCESSOCOTACAO_ID_SEQUENCE");
            //References(x => x.Produto).Column("CodigoProduto");
            //HasManyToMany(x => x.Fornecedores).Table("PROCESSOCOTACAOFORNECEDOR").ParentKeyColumn("IdProcessoCotacao").ChildKeyColumn("CodigoFornecedor")
            //    .Fetch.Join()
            //    .ExtraLazyLoad();
            //HasMany(x => x.Cotacoes).KeyColumn("IdProcessoCotacao").Cascade.AllDeleteOrphan();


            HasMany(x => x.Itens).KeyColumn("IdProcessoCotacao")
                                 .Inverse()
                                 .Cascade.AllDeleteOrphan();

            HasMany(x => x.FornecedoresParticipantes).KeyColumn("IdProcessoCotacao")
                .Inverse() /*Sem este INVERSE não funciona o delete da entidade principal: ProcessoDeCotacao. O NHibernate tenta fazer um update na tabela 
                            referente à entidade FornecedorParticipante setando o IdProcessoCotacao para NULL, o que não é permitido, pois a coluna é NOT NULL*/
                .Cascade.AllDeleteOrphan();

            //References(x => x.UnidadeDeMedida).Column("CodigoUnidadeMedida");

            References(x => x.Comprador, "LoginComprador");

            Map(x => x.Status).CustomType<Enumeradores.StatusProcessoCotacao>();
            //Map(x => x.Quantidade);
            Map(x => x.DataLimiteDeRetorno).Column("DataLimiteRetorno");
            Map(x => x.DataDeFechamento);
            Map(x => x.Requisitos);

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
            //References(x => x.RequisicaoDeCompra).Column("IdRequisicaoCompra");
            //DiscriminatorValue(Enumeradores.TipoDeCotacao.Material);
        }
    }

    public class ProcessoDeCotacaoDeFreteMap: SubclassMap<ProcessoDeCotacaoDeFrete>
    {
        public ProcessoDeCotacaoDeFreteMap()
        {
            Table("ProcessoCotacaoFrete");
            KeyColumn("Id");

            References(x => x.Itinerario).Column("CodigoItinerario");
            References(x => x.FornecedorDaMercadoria).Column("CodigoFornecedor").Nullable();
            References(x => x.MunicipioDeOrigem).Column("CodigoMunicipioOrigem");
            References(x => x.MunicipioDeDestino).Column("CodigoMunicipioDestino");
            References(x => x.Deposito).Column("CodigoDeposito");
            References(x => x.Terminal).Column("CodigoTerminal");

            Map(x => x.DataDeValidadeInicial).Column("DataValidadeInicial");
            Map(x => x.DataDeValidadeFinal).Column("DataValidadeFinal");
            Map(x => x.NumeroDoContrato).Column("NumeroContrato");
            Map(x => x.Classificacao);

        }
    }
}
