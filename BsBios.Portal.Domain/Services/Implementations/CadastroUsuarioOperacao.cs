using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CadastroUsuarioOperacao : ICadastroUsuarioOperacao
    {
        public Usuario Criar(UsuarioCadastroVm usuarioCadastroVm)
        {
            var usuario = new Usuario(usuarioCadastroVm.Nome, usuarioCadastroVm.Login,
                                      usuarioCadastroVm.Email, Enumeradores.Perfil.Comprador);
            return usuario;
        }

        public void Alterar(Usuario usuario, UsuarioCadastroVm usuarioCadastroVm)
        {
            usuario.Alterar(usuarioCadastroVm.Nome, usuarioCadastroVm.Email);
        }
    }
}