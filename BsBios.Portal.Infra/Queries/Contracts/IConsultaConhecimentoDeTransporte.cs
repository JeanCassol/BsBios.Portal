using BsBios.Portal.Common.DTO;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaConhecimentoDeTransporte
    {
        KendoGridVm Listar(PaginacaoVm paginacao, FiltroDeConhecimentoDeTransporte filtro);
        ConhecimentoDeTransporteFormulario ObterRegistro(string chaveEletronica);
        KendoGridVm ListarNotasFiscais(string chaveEletronica);
        KendoGridVm ListarOrdensDeTransporte(string chaveEletronica);
    }
}