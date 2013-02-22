using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IReadOnlyRepository<TEntidade> where TEntidade: IAggregateRoot
    {
        IList<TEntidade> List();
        TEntidade Single();
        int Count();
        IReadOnlyRepository<TEntidade> Skip(int count);
        IReadOnlyRepository<TEntidade> Take(int count);
        IQueryable<TEntidade> GetQuery();
    }
}
