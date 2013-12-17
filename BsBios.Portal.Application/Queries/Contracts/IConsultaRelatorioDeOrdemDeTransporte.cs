using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaRelatorioDeOrdemDeTransporte
    {
        IList<RelatorioDeOrdemDeTransporteAnaliticoVm> ListagemAnalitica(RelatorioDeOrdemDeTransporteFiltroVm filtro);
        IList<RelatorioDeOrdemDeTransporteSinteticoVm> ListagemSintetica(RelatorioDeOrdemDeTransporteFiltroVm filtro);
    }
}