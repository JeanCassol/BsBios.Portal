using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Implementations
{
    public class ConsultaQuota : IConsultaQuota
    {
        private readonly IQuotas _quotas;
        private readonly IBuilder<Quota, QuotaConsultarVm> _builderQuota;

        public ConsultaQuota(IQuotas quotas, IBuilder<Quota, QuotaConsultarVm> builderQuota)
        {
            _quotas = quotas;
            _builderQuota = builderQuota;
        }

        public bool PossuiQuotaNaData(DateTime data)
        {
            return _quotas.FiltraPorData(data).Count() > 0;
        }

        public IList<QuotaConsultarVm> QuotasDaData(DateTime data)
        {
            return _builderQuota.BuildList(_quotas.FiltraPorData(data).List());
        }
    }
}