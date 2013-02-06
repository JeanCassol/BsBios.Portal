using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroProduto
    {
        void Novo(ProdutoCadastroVm produtoCadastroVm);
    }
}
