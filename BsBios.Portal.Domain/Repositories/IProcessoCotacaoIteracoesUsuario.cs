using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IProcessoCotacaoIteracoesUsuario: ICompleteRepository<ProcessoCotacaoIteracaoUsuario>
    {
        ProcessoCotacaoIteracaoUsuario BuscaPorIdParticipante(int idFornecedorParticipante);
    }
}