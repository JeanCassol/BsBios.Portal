using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Infra.Queries.Builders
{

    public class UsuarioConsultaBuilder : Builder<Usuario, UsuarioConsultaVm>
    {
        public override UsuarioConsultaVm BuildSingle(Usuario model)
        {
            return new UsuarioConsultaVm()
            {
                Login = model.Login,
                Nome = model.Nome,
                Email = model.Email,
                Status = model.Status.Descricao()
            };
        }

    }
}
