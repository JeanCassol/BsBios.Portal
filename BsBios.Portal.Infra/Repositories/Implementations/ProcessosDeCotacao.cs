using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacao : CompleteRepositoryNh<ProcessoDeCotacao>, IProcessosDeCotacao
    {
        public ProcessosDeCotacao(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IProcessosDeCotacao BuscaPorId(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }


    }
}