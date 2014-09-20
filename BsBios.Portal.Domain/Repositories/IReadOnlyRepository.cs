using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IReadOnlyRepository<TEntidade> where TEntidade: IAggregateRoot
    {
        IList<TEntidade> List();
        TEntidade Single();
        int Count();
        IReadOnlyRepository<TEntidade> Skip(int count);
        IReadOnlyRepository<TEntidade> Take(int count);
        IQueryable<TEntidade> GetQuery();
        //IQueryOver<TEntidade, TEntidade> GetQueryOver();
    }
}
