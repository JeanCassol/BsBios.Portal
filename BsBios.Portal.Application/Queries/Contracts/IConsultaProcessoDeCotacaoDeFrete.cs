using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaProcessoDeCotacaoDeFrete
        {
            ProcessoCotacaoFreteCadastroVm ConsultaProcesso(int idProcessoCotacaoFrete);
            IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem);
            KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro);
        }
}