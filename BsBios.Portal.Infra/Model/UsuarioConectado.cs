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
        public int Id { get; set; }
        public string NomeCompleto { get; set; }
        public int Perfil { get; set; }
        public UsuarioConectado(int id, string nomeCompleto, int perfil)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Perfil = perfil;
        }
        
    }
}
