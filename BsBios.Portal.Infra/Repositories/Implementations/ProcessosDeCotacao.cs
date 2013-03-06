using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ProcessosDeCotacao : CompleteRepositoryNh<ProcessoDeCotacao>, IProcessosDeCotacao
    {
        public ProcessosDeCotacao(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IProcessosDeCotacao BuscaPorId(int id)
        {
            Query = Query.Where(x => x.Id == id);
            return this;
        }

        public IProcessosDeCotacao FiltraPorFornecedor(string codigoFornecedor)
        {
            //Query = (from p in Query
            //                 from fp in p.FornecedoresParticipantes
            //                 where fp.Fornecedor.Codigo == codigoFornecedor
            //                     select p);

            Query = Query.Where(x => x.FornecedoresParticipantes.Any(y => y.Fornecedor.Codigo == codigoFornecedor));

            return this;
        }

        public IProcessosDeCotacao DesconsideraNaoIniciados()
        {
            Query = Query.Where(x => x.Status != Enumeradores.StatusProcessoCotacao.NaoIniciado);
            return this;
        }
    }
}