using BsBios.Portal.ViewModel;

namespace BsBios.Portal.ApplicationServices.Contracts
{
    public interface ICadastroUsuario
    {
        void Novo(UsuarioVm usuarioVm);
    }
}