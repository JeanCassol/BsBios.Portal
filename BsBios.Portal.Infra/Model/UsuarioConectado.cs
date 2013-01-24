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
        public Enumeradores.Perfil Perfil { get; set; }
        public UsuarioConectado(int id, string nomeCompleto, Enumeradores.Perfil perfil)
        {
            Id = id;
            NomeCompleto = nomeCompleto;
            Perfil = perfil;
        }
        
    }
}
