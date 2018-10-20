using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface ICotacaoHistoricoRepository: ICompleteRepository<CotacaoHistorico>
    {
        ICotacaoHistoricoRepository DoFornecedorParticipante(int idFornecedorParticipante);
    }
}