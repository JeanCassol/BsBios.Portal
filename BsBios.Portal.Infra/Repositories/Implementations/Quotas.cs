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

        public IQuotas FiltraPorTransportadora(string codigoTransportadora)
        {
            Query = Query.Where(x => x.Transportadora.Codigo == codigoTransportadora);
            return this;
        }

        public IQuotas FiltraPorFluxo(Enumeradores.FluxoDeCarga fluxoDeCarga)
        {
            Query = Query.Where(x => x.FluxoDeCarga == fluxoDeCarga);
            return this;
        }
    }
}