using System;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaQuota
    {
        bool PossuiQuotaNaData(DateTime data);
    }
}