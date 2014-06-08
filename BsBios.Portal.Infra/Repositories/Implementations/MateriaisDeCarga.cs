using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class MateriaisDeCarga : ReadOnlyRepositoryNh<MaterialDeCarga>, IMateriaisDeCarga
    {
        public MateriaisDeCarga(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IMateriaisDeCarga BuscarLista(int[] codigos)
        {
            Query = Query.Where(m => codigos.Contains(m.Codigo));
            return this;
        }

        public IMateriaisDeCarga BuscaPorCodigo(int codigo)
        {
            Query = Query.Where(x => x.Codigo == codigo);

            return this;
        }
    }
}