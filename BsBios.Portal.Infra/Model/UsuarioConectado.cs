using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain;

namespace BsBios.Portal.Infra.Model
{
    public class UsuarioConectado
    {
        public string Login { get; set; }
        public string NomeCompleto { get; set; }
        public int Perfil { get; set; }
        public UsuarioConectado(string login, string nomeCompleto, int perfil)
        {
            Login = login;
            NomeCompleto = nomeCompleto;
            Perfil = perfil;
        }
        
    }
}
