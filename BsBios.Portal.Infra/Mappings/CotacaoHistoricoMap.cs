using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class CotacaoHistoricoMap: ClassMap<CotacaoHistorico>
    {
        public CotacaoHistoricoMap()
        {
            Table("CotacaoHistorico");
            Id(x => x.Id).GeneratedBy.Sequence("COTACAO_HISTORICO_ID_SEQUENCE");
            Map(x => x.IdFornecedorParticipante);
            Map(x => x.Usuario);
            Map(x => x.DataHora);
            Map(x => x.Descricao);
        }
    }
}