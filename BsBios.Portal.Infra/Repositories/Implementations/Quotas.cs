using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class Quotas : CompleteRepositoryNh<Quota>,  IQuotas
    {
        public Quotas(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IQuotas FiltraPorData(DateTime data)
        {
            Query = Query.Where(x => x.Data == data);
            return this;
        }

        public IQuotas APartirDe(DateTime dataInicial)
        {
            Query = Query.Where(x => x.Data >= dataInicial);
            return this;
        }

        public IQuotas Ate(DateTime dataFinal)
        {
            Query = Query.Where(x => x.Data <= dataFinal);
            return this;
        }

        public IQuotas FiltraPorFluxo(Enumeradores.FluxoDeCarga fluxoDeCarga)
        {
            Query = Query.Where(x => x.FluxoDeCarga == fluxoDeCarga);
            return this;
        }

        public IQuotas DoFornecedor(string codigoDoFornecedor)
        {
            Query = Query.Where(x => x.Fornecedor.Codigo == codigoDoFornecedor);
            return this;
        }

        public IQuotas DoTerminal(string codigoDoTerminal)
        {
            if (!string.IsNullOrEmpty(codigoDoTerminal))
            {
                Query = Query.Where(x => x.Terminal.Codigo == codigoDoTerminal);
            }
            return this;
        }

        public Quota BuscaPorId(int idQuota)
        {
            return Query.Single(x => x.Id == idQuota);
        }
    }
}