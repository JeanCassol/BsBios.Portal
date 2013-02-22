using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICadastroUsuarioOperacao
    {
        Usuario Criar(UsuarioCadastroVm usuarioCadastroV);
        void Alterar(Usuario usuario, UsuarioCadastroVm usuarioCadastroVm);
    }
}