using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaMonitorDeOrdemDeTransporte
    {
        IList<MonitorDeOrdemDeTransportePorMaterialVm> ListarPorMaterial(MonitorDeOrdemDeTransporteConfiguracaoVm filtro);
    }
}