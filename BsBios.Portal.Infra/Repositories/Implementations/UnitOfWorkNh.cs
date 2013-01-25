using System.Data;
using BsBios.Portal.Infra.Repositories.Contracts;
using NHibernate;

namespace BsBios.Portal.Infra.Repositories.Implementations
{

    public class UnitOfWorkNh: IUnitOfWorkNh
    {
        public ISession Session { get; private set; }

        public UnitOfWorkNh(ISessionFactory sessionFactory)
        {
            SessionFactory = sessionFactory;
            Session = SessionFactory.OpenSession();
        }

        protected readonly ISessionFactory SessionFactory;
        private ITransaction _transaction;
        public void Dispose()
        {
            if (Session != null && Session.IsOpen)
            {
                Session.Clear();
            }
        }

        public void BeginTransaction()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                return;
            }
            _transaction = Session.BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Commit()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                _transaction.Commit();
            }
        }

        public void RollBack()
        {
            if (_transaction != null && _transaction.IsActive)
            {
                _transaction.Rollback();
            }
        }

    }
}
