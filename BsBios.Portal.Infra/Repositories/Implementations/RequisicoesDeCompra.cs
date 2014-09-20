using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class RequisicoesDeCompra: CompleteRepositoryNh<RequisicaoDeCompra>, IRequisicoesDeCompra
    {
        public RequisicoesDeCompra(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public RequisicaoDeCompra BuscaPeloId(int id)
        {
            return Query.SingleOrDefault(x => x.Id == id);
        }
    }
}
