using System;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaQuota : IConsultaQuota
    {
        private readonly IQuotas _quotas;

        public ConsultaQuota(IQuotas quotas)
        {
            _quotas = quotas;
        }

        public bool PossuiQuotaNaData(DateTime data)
        {
            return _quotas.FiltraPorData(data).Count() > 0;
        }
    }
}