using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface ITerminais: ICompleteRepository<Terminal>
    {
        Terminal BuscaPeloCodigo(string codigo);
    }
}