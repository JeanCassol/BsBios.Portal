using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface ITerminais: ICompleteRepository<Terminal>
    {
        Terminal BuscaPeloCodigo(string codigo);
        ITerminais BuscaListaPorCodigo(string[] codigos);
    }
}