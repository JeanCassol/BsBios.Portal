using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaEficienciaNegociacao
    {
        //IList<RelatorioEficienciaNegociacaoVm> Consultar(RelatorioEficienciaNegociacaoFiltroVm filtro);
        IList<ProcessoDeCotacaoValoresVm> Consultar(RelatorioEficienciaNegociacaoFiltroVm filtro);
    }
}