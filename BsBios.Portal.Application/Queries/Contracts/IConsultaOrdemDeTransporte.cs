using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaOrdemDeTransporte
    {
        KendoGridVm Listar(PaginacaoVm paginacao, OrdemDeTransporteListagemFiltroVm filtro);
        OrdemDeTransporteCadastroVm ConsultarOrdem(int id);
    }
}