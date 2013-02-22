using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacaoDeMaterial : CompleteRepositoryNh<ProcessoDeCotacao>, IProcessosDeCotacaoDeMaterial
    {
        public ProcessosDeCotacaoDeMaterial(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public ProcessoDeCotacao BuscaPorId(int id)
        {
            return Query.SingleOrDefault(x => x.Id == id);
        }
    }
}