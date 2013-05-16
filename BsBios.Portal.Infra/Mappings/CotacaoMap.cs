using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class CotacaoMap : ClassMap<Cotacao>
    {
        public CotacaoMap()
        {
            Table("Cotacao");
            //DynamicInsert();
            Id(x => x.Id).GeneratedBy.Sequence("COTACAO_ID_SEQUENCE");

            //Tive problemas com o composite Id na hora de excluir registros. Acho que o modelo do banco não estava bom: 
            //Tinha uma FK para a tabela "PROCESSOCOTACAOFORNECEDOR", mas tabela "PROCESSOCOTACAOFORNECEDOR" não apontava para esta
            //CompositeId()
            //    .KeyReference(x => x.ProcessoDeCotacao, "IdProcessoCotacao")
            //    .KeyReference(x => x.Fornecedor, "CodigoFornecedor");
            
            //Usei este tipo de Id para tentar criar um relacionamento one-to-one, mas tive problemas porque queria que a propriedade
            //Cotacao na entidade FornecedorParticipante inicializasse como null e fosse criada só depois. O NHibernate estava sempre
            //inserindo um registro na tabela Cotacao com todos os campos null, menos as chaves, mesmo que a propriedade na classe 
            //FornecedorParticipante estivesse nula
            //Id(Reveal.Member<Cotacao>("IdFornecedorParticipante")).GeneratedBy.Foreign("FornecedorParticipante");

            //HasOne(x => x.FornecedorParticipante).Constrained().ForeignKey();

            //References(x => x.FornecedorParticipante).Column("IdFornecedorParticipante");

            HasMany(x => x.Itens).KeyColumn("IdCotacao")
                                 .Inverse()
                                 .Cascade.AllDeleteOrphan();
            
        }
    }
    public class CotacaoMaterialMap: SubclassMap<CotacaoMaterial>
    {
        public CotacaoMaterialMap()
        {
            Table("CotacaoMaterial");
            KeyColumn("Id");
            References(x => x.CondicaoDePagamento).Column("CodigoCondicaoPagamento");
            References(x => x.Incoterm).Column("CodigoIncoterm");
            Map(x => x.DescricaoIncoterm);
            Map(x => x.TipoDeFrete).CustomType<Enumeradores.TipoDeFrete>();
        }
    }
    public class CotacaoFreteMap : SubclassMap<CotacaoFrete>
    {
        public CotacaoFreteMap()
        {
            Table("CotacaoFrete");
            KeyColumn("Id");
        }
    }
    
}
