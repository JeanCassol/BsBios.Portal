using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IAgendamentosDeCarga : ICompleteRepository<AgendamentoDeCarga>
    {
        AgendamentoDeCarga BuscaPorId(int id);
    }
}