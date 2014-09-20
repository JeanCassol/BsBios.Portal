using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IMateriaisDeCarga: IReadOnlyRepository<MaterialDeCarga>
    {
        IMateriaisDeCarga BuscarLista(int[] codigos);
        IMateriaisDeCarga BuscaPorCodigo(int codigo);
    }
}