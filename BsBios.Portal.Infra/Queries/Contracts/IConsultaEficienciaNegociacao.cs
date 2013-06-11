using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaEficienciaNegociacao
    {
        IList<EficienciaDeNegociacaoResumoVm> Consultar(PaginacaoVm paginacaoVm, EficienciaNegociacaoFiltroVm filtro);
    }
}