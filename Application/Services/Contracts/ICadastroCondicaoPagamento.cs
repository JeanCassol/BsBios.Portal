using BsBios.Portal.ViewModel;

namespace Application.Services.Contracts
{
    public interface ICadastroCondicaoPagamento
    {
        void Novo(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm);
    }
}