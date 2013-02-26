using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IProcessosDeCotacao:ICompleteRepository<ProcessoDeCotacao>
    {
        IProcessosDeCotacao BuscaPorId(int id);
    }
}
