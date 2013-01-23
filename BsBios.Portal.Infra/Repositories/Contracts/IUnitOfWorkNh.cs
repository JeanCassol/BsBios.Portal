using NHibernate;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUnitOfWorkNh: IUnitOfWork
    {
        ISession Session { get; }         
    }
}