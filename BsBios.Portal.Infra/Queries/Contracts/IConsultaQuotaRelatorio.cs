using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaQuotaRelatorio
    {
        RelatorioDeQuotaPlanejadoVersusRealizadoVm PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro);
        RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm PlanejadoRealizadoPorData(RelatorioAgendamentoFiltroVm filtro);
        IList<QuotaListagemVm> ListagemDeQuotas(RelatorioAgendamentoFiltroVm filtro);
        IList<AgendamentoDeCargaRelatorioListarVm> ListagemDeAgendamentos(RelatorioAgendamentoFiltroVm filtro);
    }
}