using BsBios.Portal.Domain.Model;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface IAtualizadorProduto
    {
        Produto Novo(ProdutoCadastroVm produtoCadastroVm);
        void Atualizar(Produto produto, ProdutoCadastroVm novosDados);
    }
}