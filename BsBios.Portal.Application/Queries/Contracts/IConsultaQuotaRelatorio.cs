using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaQuotaRelatorio
    {
        RelatorioDeQuotaPlanejadoVersusRealizadoVm PlanejadoRealizado(RelatorioAgendamentoFiltroVm filtro);
        RelatorioDeQuotaPlanejadoVersusRealizadoPorDataVm PlanejadoRealizadoPorData(RelatorioAgendamentoFiltroVm filtro);
        IList<QuotaCadastroVm> ListagemDeQuotas(RelatorioAgendamentoFiltroVm filtro);
        IList<AgendamentoDeCargaRelatorioListarVm> ListagemDeAgendamentos(RelatorioAgendamentoFiltroVm filtro);
    }
}