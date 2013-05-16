using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaCotacaoDoFornecedor
    {
        CotacaoMaterialCadastroVm ConsultarCotacaoDeMaterial(int idProcessoCotacao, string codigoFornecedor);
        CotacaoMaterialConsultarCadastroVm ConsultarCotacaoDeMaterialParaVisualizacao(int idProcessoCotacao, string codigoFornecedor);
        CotacaoMaterialItemCadastroVm ConsultarCotacaoDeItemDeMaterial(int idProcessoCotacao, string codigoFornecedor,string numeroDaRequisicao, string numeroDoItemDaRequisicao);
        CotacaoFreteCadastroVm ConsultarCotacaoDeFrete(int idProcessoCotacao, string codigoFornecedor);
    }
}