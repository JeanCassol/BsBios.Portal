using BsBios.Portal.Infra.Repositories.Contracts;
using Moq;

namespace BsBios.Portal.Tests.DefaultProvider
{
    public static class DefaultRepository
    {
        public static Mock<IUnitOfWork> GetDefaultMockUnitOfWork()
        {
            var unitOfWorkNhMock = new Mock<IUnitOfWork>(MockBehavior.Strict);
            unitOfWorkNhMock.Setup(x => x.BeginTransaction());

            //callback garante que a transação sempre é iniciada (BeginTran) antes de fazer Commit
            unitOfWorkNhMock.Setup(x => x.Commit())
                .Callback(() => unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once()));

            //callback garante que a transação sempre é iniciada (BeginTran) antes de fazer Rollback
            unitOfWorkNhMock.Setup(x => x.RollBack())
                .Callback(() => unitOfWorkNhMock.Verify(x => x.BeginTransaction(), Times.Once()));

            return unitOfWorkNhMock;
        }
    }
}
