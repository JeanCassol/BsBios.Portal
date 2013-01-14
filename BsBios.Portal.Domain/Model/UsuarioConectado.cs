using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain.Model
{
    public class UsuarioConectado
    {
        public UsuarioConectado(string nomeCompleto, Perfil perfil)
        {
            NomeCompleto = nomeCompleto;
            Perfil = perfil;
        }

        public string NomeCompleto { get; set; }
        public Perfil Perfil { get; set; }
    }
}
