using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroCondicaoPagamento
    {
        void Novo(CondicaoDePagamentoCadastroVm condicaoDePagamentoCadastroVm);
    }
}