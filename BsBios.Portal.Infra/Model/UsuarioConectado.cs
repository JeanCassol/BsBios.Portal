using System.Collections.Generic;
using BsBios.Portal.Common;

namespace BsBios.Portal.Infra.Model
{
    public class UsuarioConectado
    {
        public string Login { get; set; }
        public string NomeCompleto { get; set; }
        public IList<Enumeradores.Perfil> Perfis { get; set; }
        public UsuarioConectado(string login, string nomeCompleto, IList<Enumeradores.Perfil>perfis )
        {
            Login = login;
            NomeCompleto = nomeCompleto;
            Perfis = perfis;
        }
        
    }
}
