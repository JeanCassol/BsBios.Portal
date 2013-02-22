using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICadastroProdutoOperacao
    {
        Produto Criar(ProdutoCadastroVm produtoCadastroVm);
        void Atualizar(Produto produto, ProdutoCadastroVm novosDados);
    }
}