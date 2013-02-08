using System.Linq;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class CondicoesDePagamento: CompleteRepositoryNh<CondicaoDePagamento>, ICondicoesDePagamento
    {
        public CondicoesDePagamento(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public CondicaoDePagamento BuscaPeloCodigoSap(string codigoSap)
        {
            return Query.SingleOrDefault(x => x.CodigoSap == codigoSap);
        }
    }
}
