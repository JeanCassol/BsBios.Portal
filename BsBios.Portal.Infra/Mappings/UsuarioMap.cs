using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using FluentNHibernate.Mapping;

namespace BsBios.Portal.Infra.Mappings
{
    public class UsuarioMap: ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            Table("Usuario");
            Id(x => x.Login);
            Map(u => u.Nome);
            Map(u => u.Senha);
            Map(u => u.Email);
            Map(u => u.Perfil).CustomType<Enumeradores.Perfil>();
        }
    }
}
