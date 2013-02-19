using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CadastroProdutoOperacao : ICadastroProdutoOperacao
    {
        public Produto Criar(ProdutoCadastroVm produtoCadastroVm)
        {
            var produto = new Produto(produtoCadastroVm.CodigoSap, produtoCadastroVm.Descricao, produtoCadastroVm.Tipo);
            return produto;
        }

        public void Atualizar(Produto produto, ProdutoCadastroVm novosDados)
        {
            produto.Atualizar(novosDados.Descricao, novosDados.Tipo);
        }
    }
}