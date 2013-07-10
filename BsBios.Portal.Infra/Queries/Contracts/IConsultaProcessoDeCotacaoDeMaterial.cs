using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Contracts
{
    public interface IConsultaProcessoDeCotacaoDeMaterial
        {
            KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoCotacaoFiltroVm filtro);
            ProcessoCotacaoMaterialCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial);
            KendoGridVm FornecedoresParticipantes(int idProcessoCotacao);
            IList<CotacaoMaterialSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem);
            IList<FornecedorCotacaoVm> CotacoesDetalhadaDosFornecedores(int idProcessoCotacao, int idProcessoCotacaoItem);
            KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao);
            KendoGridVm ListarItens(int idProcessoCotacao);
            string[] CodigoDosProdutos(int idProcessoCotacao);
            FornecedorVm[] ListarFornecedores(int idProcessoCotacao);
        }
}