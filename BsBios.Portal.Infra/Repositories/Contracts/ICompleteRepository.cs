using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface ICompleteRepository<TEntidade> : IReadOnlyRepository<TEntidade> where TEntidade:IAggregateRoot
    {
        void Save(TEntidade entidade);
        void Delete(TEntidade entidade);
    }
}
