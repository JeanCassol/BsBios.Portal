using BsBios.Portal.Application.DTO;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaConhecimentoDeTransporte
    {
        KendoGridVm Listar(PaginacaoVm paginacao, FiltroDeConhecimentoDeTransporte filtro);
    }
}