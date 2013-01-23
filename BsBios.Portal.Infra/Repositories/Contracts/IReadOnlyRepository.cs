using System.Collections.Generic;
using BsBios.Portal.Domain.Model;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IReadOnlyRepository<TEntidade> where TEntidade: IAggregateRoot
    {
        IList<TEntidade> List();
        TEntidade Single();
    }
}
