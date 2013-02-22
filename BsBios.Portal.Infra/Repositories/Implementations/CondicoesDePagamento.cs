using System.Linq;
using BsBios.Portal.Domain.Entities;
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
            return Query.SingleOrDefault(x => x.Codigo == codigoSap);
        }

        public ICondicoesDePagamento FiltraPelaDescricao(string descricao)
        {
            Query = Query.Where(x => x.Descricao.ToLower().Contains(descricao.ToLower()));
            return this;
        }
    }
}
