using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroCondicaoPagamento
    {
        void Novo(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm);
    }
}