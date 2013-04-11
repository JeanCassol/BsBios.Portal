using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaQuotaRelatorio
    {
        IList<QuotaPlanejadoRealizadoVm> PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro);
    }
}