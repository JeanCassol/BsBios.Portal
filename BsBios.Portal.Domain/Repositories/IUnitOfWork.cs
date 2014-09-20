using System;

namespace BsBios.Portal.Domain.Repositories
{
    public interface IUnitOfWork:IDisposable
    {
        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
