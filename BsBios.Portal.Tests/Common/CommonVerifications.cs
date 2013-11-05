using BsBios.Portal.Infra.Repositories.Contracts;
using Moq;

namespace BsBios.Portal.Tests.Common
{
    public static class CommonVerifications
    {
        public static void VerificaCommitDeTransacao(Mock<IUnitOfWork> unitOfWorkMock)
        {
            unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
        }
        public static void VerificaRollBackDeTransacao(Mock<IUnitOfWork> unitOfWorkMock)
        {
            unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
            unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
        }

    }
}
