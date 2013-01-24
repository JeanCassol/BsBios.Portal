using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUsuarios: ICompleteRepository<Usuario>
    {
        Usuario BuscaPorId(int idUsuario);
        Usuario BuscaPorLogin(string login);
    }
}