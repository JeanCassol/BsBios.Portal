using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
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
