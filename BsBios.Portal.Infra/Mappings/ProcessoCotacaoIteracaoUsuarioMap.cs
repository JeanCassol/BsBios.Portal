using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class ProcessoCotacaoIteracaoUsuarioMap: ClassMap<ProcessoCotacaoIteracaoUsuario>
    {
        public ProcessoCotacaoIteracaoUsuarioMap()
        {
            Table("ProcessoCotacaoIteracaoUsuario");
            Id(x => x.IdFornecedorParticipante).GeneratedBy.Assigned();
            Map(x => x.VisualizadoPeloFornecedor);
        }
    }
}
