using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Queries.Contracts
{
    public interface IConsultaCotacaoDoFornecedor
    {
        CotacaoCadastroVm ConsultarCotacao(int idProcessoCotacao, string codigoFornecedor);
    }
}