using BsBios.Portal.Domain.Repositories;
using NHibernate;

namespace BsBios.Portal.Infra.Repositories
{
    public interface IUnitOfWorkNh: IUnitOfWork
    {
        ISession Session { get; }         
    }
}