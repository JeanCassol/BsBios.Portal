using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaProcessoDeCotacaoDeFrete
        {
            ProcessoCotacaoFreteCadastroVm ConsultaProcesso(int idProcessoCotacao);
            IList<CotacaoSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao);
            KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro);
            KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao);
        }
}