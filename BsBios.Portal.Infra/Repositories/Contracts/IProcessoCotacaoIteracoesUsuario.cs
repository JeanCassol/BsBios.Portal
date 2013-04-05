using BsBios.Portal.Infra.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessoCotacaoIteracoesUsuario: ICompleteRepository<ProcessoCotacaoIteracaoUsuario>
    {
        ProcessoCotacaoIteracaoUsuario BuscaPorIdParticipante(int idFornecedorParticipante);
    }
}