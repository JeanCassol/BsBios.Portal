using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Model;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class UsuarioMap: ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");
            Id(x => x.Id).GeneratedBy.Sequence("USUARIO_ID_SEQUENCE");
            Map(u => u.Nome);
            Map(u => u.Login);
            Map(u => u.Senha);
            Map(u => u.Email);
        }
    }
}
