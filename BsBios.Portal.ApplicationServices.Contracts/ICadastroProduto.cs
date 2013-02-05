using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroProduto
    {
        void Novo(ProdutoCadastroVm produtoCadastroVm);
    }
}
