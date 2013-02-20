using System.Collections.Generic;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface ICadastroUsuario
    {
        void Novo(UsuarioCadastroVm usuarioVm);
        void AtualizarUsuarios(IList<UsuarioCadastroVm> usuarios);
    }
}