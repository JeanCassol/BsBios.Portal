using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IGerenciadorUsuario
    {
        UsuarioConsultaVm CriarSenha(string login);
        void AlterarSenha(string login, string senhaAtual, string senhaNova);
        void Ativar(string login);
        void Bloquear(string login);
        void AtualizarPerfis(string login, IList<Enumeradores.Perfil> perfis);
    }
}