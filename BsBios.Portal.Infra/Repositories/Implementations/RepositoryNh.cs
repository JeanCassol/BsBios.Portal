using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using NHibernate.Linq;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public abstract class RepositoryNh<TEntity> where TEntity: IAggregateRoot
    {
        protected IUnitOfWorkNh UnitOfWorkNh;
        protected IQueryable<TEntity> Query;

        protected RepositoryNh(IUnitOfWorkNh unitOfWorkNh)
        {
            UnitOfWorkNh = unitOfWorkNh;
            Query = UnitOfWorkNh.Session.Query<TEntity>();
        }
    }
}
