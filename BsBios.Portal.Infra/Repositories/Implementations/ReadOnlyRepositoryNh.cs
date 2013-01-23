using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class ReadOnlyRepositoryNh<TEntity>: RepositoryNh<TEntity>, IReadOnlyRepository<TEntity> where TEntity:IAggregateRoot
    {
        public ReadOnlyRepositoryNh(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public IList<TEntity> List()
        {
            return Query.ToList();
        }

        public TEntity Single()
        {
            return Query.SingleOrDefault();
        }
    }
}
