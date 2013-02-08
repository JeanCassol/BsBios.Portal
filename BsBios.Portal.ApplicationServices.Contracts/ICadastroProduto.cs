using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroProduto
    {
        void Novo(ProdutoCadastroVm produtoCadastroVm);
        /// <summary>
        /// recebe uma lista de produtos: se o registro já existir atualiza senão cria um registro novo.
        /// </summary>
        /// <param name="produtos">lista de produtos</param>
        void AtualizarProdutos(IList<ProdutoCadastroVm> produtos);
    }
}
