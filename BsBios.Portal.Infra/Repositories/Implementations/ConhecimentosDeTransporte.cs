using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using NHibernate.Linq;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ConhecimentosDeTransporte: CompleteRepositoryNh<ConhecimentoDeTransporte>, IConhecimentosDeTransporte
    {

        public ConhecimentosDeTransporte(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IConhecimentosDeTransporte ComChaveEletronica(string chave)
        {
            Query = Query.Where(ct => ct.ChaveEletronica == chave);
                
            return this;
        }

        public IConhecimentosDeTransporte IncluirNotasFiscais()
        {
            Query = Query.Fetch(ConhecimentoDeTransporte.Expressions.NotasFiscais);
            return this;
        }
    }
}
