using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;

namespace BsBios.Portal.Infra.Repositories.Implementations
{
    public class CompleteRepositoryNh<TEntity>: ReadOnlyRepositoryNh<TEntity>, ICompleteRepository<TEntity> where TEntity:IAggregateRoot
    {
        public CompleteRepositoryNh(IUnitOfWorkNh unitOfWork) : base(unitOfWork)
        {
        }

        public void Save(TEntity entidade)
        {
            //UnitOfWorkNh.Session.SaveOrUpdate(entidade);
            UnitOfWorkNh.Session.Save(entidade);
        }

        public void Delete(TEntity entidade)
        {
            UnitOfWorkNh.Session.Delete(entidade);
        }
    }
}
