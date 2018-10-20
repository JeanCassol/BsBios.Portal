using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class CotacaoHistoricos: CompleteRepositoryNh<CotacaoHistorico>, ICotacaoHistoricoRepository
    {
        public CotacaoHistoricos(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public ICotacaoHistoricoRepository DoFornecedorParticipante(int idFornecedorParticipante)
        {
            Query = Query.Where(historico => historico.IdFornecedorParticipante == idFornecedorParticipante);
            return this;
        }
    }
}