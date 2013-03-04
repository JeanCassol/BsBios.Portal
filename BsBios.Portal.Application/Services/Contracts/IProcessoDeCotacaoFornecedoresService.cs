using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IProcessoDeCotacaoFornecedoresService
    {
        void AtualizarFornecedores(AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm atualizacaoDosFornecedoresVm);
         
    }
}