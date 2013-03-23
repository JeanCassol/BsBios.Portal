using System;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IQuotas: ICompleteRepository<Quota>
    {
        IQuotas FiltraPorData(DateTime data);
        IQuotas FiltraPorTransportadora(string codigoTransportadora);
        IQuotas FiltraPorFluxo(Enumeradores.FluxoDeCarga fluxoDeCarga);
        IQuotas DoFornecedor(string codigoDoFornecedor);
    }
}