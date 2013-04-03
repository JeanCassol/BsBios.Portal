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
        /// <summary>
        /// recebe uma lista de usuários e verifica se cada um dos usuários possui senha. 
        /// Para os usuários que não possuem senha gera um nova senha aleatória e envia por e-mail.
        /// </summary>
        /// <param name="logins">lista de logins para verificar a senha</param>
        void CriarSenhaParaUsuariosSemSenha(string[] logins);
    }
}