using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IMateriaisDeCarga: IReadOnlyRepository<MaterialDeCarga>
    {
        IMateriaisDeCarga BuscarLista(int[] codigos);
        IMateriaisDeCarga BuscaPorCodigo(int codigo);
    }
}