using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaRelatorioDeOrdemDeTransporte
    {
        IList<RelatorioDeOrdemDeTransporteAnaliticoVm> ListagemAnalitica(RelatorioDeOrdemDeTransporteFiltroVm filtro);
    }
}