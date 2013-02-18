using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroFornecedor
    {
        void Novo(FornecedorCadastroVm fornecedorCadastroVm);
    }
}