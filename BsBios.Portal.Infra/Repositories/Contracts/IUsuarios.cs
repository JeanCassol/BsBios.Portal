using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUsuarios: ICompleteRepository<Usuario>
    {
        IUsuarios BuscaPorId(int idUsuario);

    }
}