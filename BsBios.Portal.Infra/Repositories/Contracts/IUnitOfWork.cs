using System;
using System.Linq;

namespace BsBios.Portal.Infra.Repositories.Contracts
{
    public interface IUnitOfWork:IDisposable
    {
        void BeginTransaction();
        void Commit();
        void RollBack();
    }
}
