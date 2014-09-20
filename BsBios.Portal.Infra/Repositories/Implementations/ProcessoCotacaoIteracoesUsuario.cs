using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Model;

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