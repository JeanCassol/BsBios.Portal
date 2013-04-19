using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Repositories.Implementations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Common
{
    public static class CommonGenericMocks<T> where T: class, IAggregateRoot
    {
        public static Mock<CompleteRepositoryNh<T>> MockRepository(Mock<IUnitOfWork> unitOfWorkMock)
        {
            var mock = new Mock<CompleteRepositoryNh<T>>(MockBehavior.Strict);
            mock.Setup(x => x.Save(It.IsAny<T>())).Callback(
                (T entidade) =>
                    {
                        Assert.IsNotNull(entidade);
                        unitOfWorkMock.Verify(y => y.BeginTransaction(), Times.Once());
                        unitOfWorkMock.Verify(y => y.Commit(), Times.Never());
                    });

            return mock;
        }

        public static Action<T> DefaultSaveCallBack(Mock<IUnitOfWork> unitOfWorkMock)
        {
            return e =>
                {
                    Assert.IsNotNull(e);
                    unitOfWorkMock.Verify(y => y.BeginTransaction(), Times.Once());
                    unitOfWorkMock.Verify(y => y.Commit(), Times.Never());
                };
        }
    }
}
