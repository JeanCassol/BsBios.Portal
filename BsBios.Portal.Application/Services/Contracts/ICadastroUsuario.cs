using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroUsuario
    {
        void Novo(UsuarioVm usuarioVm);
    }
}