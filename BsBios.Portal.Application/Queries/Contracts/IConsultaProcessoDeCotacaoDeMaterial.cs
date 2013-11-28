using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaProcessoDeCotacaoDeMaterial
        {
            KendoGridVm Listar(PaginacaoVm paginacaoVm, ProcessoDeCotacaoDeFreteFiltroVm filtro);
            ProcessoCotacaoMaterialCadastroVm ConsultaProcesso(int idProcessoCotacaoMaterial);
            KendoGridVm FornecedoresParticipantes(int idProcessoCotacao);
            IList<CotacaoMaterialSelecionarVm> CotacoesDosFornecedores(int idProcessoCotacao);
            KendoGridVm CotacoesDosFornecedoresResumido(int idProcessoCotacao);
        }
}