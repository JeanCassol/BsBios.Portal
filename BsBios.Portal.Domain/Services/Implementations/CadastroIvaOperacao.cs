using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CadastroIvaOperacao : ICadastroIvaOperacao
    {
        public Iva Criar(IvaCadastroVm ivaCadastroVm)
        {
            var iva = new Iva(ivaCadastroVm.Codigo, ivaCadastroVm.Descricao);
            return iva;
        }

        public void Alterar(Iva iva, IvaCadastroVm ivaCadastroVm)
        {
            iva.AtualizaDescricao(ivaCadastroVm.Descricao);
        }
    }
}