using System.Linq;
using BsBios.Portal.Common;
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
            Query = Query.Fetch(x => x.NotasFiscais);
            return this;
        }

        public IConhecimentosDeTransporte ComErroOuSemOrdemDeTransporte()
        {
            Query = Query.Where(x => x.Status == Enumeradores.StatusDoConhecimentoDeTransporte.Erro || x.Status == Enumeradores.StatusDoConhecimentoDeTransporte.SemOrdemDeTransporte);
            return this;
        }
    }
}
