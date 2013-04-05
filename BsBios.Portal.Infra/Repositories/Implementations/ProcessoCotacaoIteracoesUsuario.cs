using System.Linq;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessoCotacaoIteracoesUsuario : CompleteRepositoryNh<ProcessoCotacaoIteracaoUsuario>, IProcessoCotacaoIteracoesUsuario
    {
        public ProcessoCotacaoIteracoesUsuario(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public ProcessoCotacaoIteracaoUsuario BuscaPorIdParticipante(int idFornecedorParticipante)
        {
            return Query.SingleOrDefault(x => x.IdFornecedorParticipante == idFornecedorParticipante);
        }
    }
}