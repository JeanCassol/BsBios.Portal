using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Infra.Repositories.Contracts;
using Moq;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultRepository
    {
        public static Mock<IUnitOfWorkNh> GetDefaultMockUnitOfWork()
        {
            var unitOfWorkNhMock = new Mock<IUnitOfWorkNh>(MockBehavior.Strict);
            unitOfWorkNhMock.Setup(x => x.BeginTransaction());
            unitOfWorkNhMock.Setup(x => x.Commit());
            unitOfWorkNhMock.Setup(x => x.RollBack());

            return unitOfWorkNhMock;
        }
    }
}
