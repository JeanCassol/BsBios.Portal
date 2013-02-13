using BsBios.Portal.ApplicationServices.Contracts;
using BsBios.Portal.ApplicationServices.Implementation;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application
{
    [TestClass]
    public class CadastroIvaTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IIvas> _ivasMock;
        private readonly ICadastroIva _cadastroIva;
        private readonly IvaCadastroVm _ivaPadrao;
        private readonly Mock<Iva> _ivaMock; 

        public CadastroIvaTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _ivasMock = new Mock<IIvas>(MockBehavior.Strict);
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>()));
            _cadastroIva = new CadastroIva(_unitOfWorkMock.Object, _ivasMock.Object);
            _ivaPadrao = new IvaCadastroVm()
                {
                    CodigoSap = "01",
                    Descricao = "IVA 01"
                };

            _ivaMock = new Mock<Iva>(MockBehavior.Strict);
            _ivaMock.Setup(x => x.AtualizaDescricao(It.IsAny<string>()));
        }

        [TestMethod]
        public void QuandoCadastraUmNovoIvaOcorrePersistencia()
        {
            _cadastroIva.Novo(_ivaPadrao);
            _ivasMock.Verify(x => x.Save(It.IsAny<Iva>()),Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmNovoIvaComSucessoFazCommitNaTransacao()
        {
            _cadastroIva.Novo(_ivaPadrao);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());       
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoNoCadastroDoIvaFazRollBackNaTransacao()
        {
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>()))
                     .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o Iva"));

            try
            {
                _cadastroIva.Novo(_ivaPadrao);
                Assert.Fail("Deveria ter gerado exceção");

            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
            }
        }

        [TestMethod]
        public void QuandoReceberUmIvaExistenteDeveAtualizar()
        {

            Assert.Fail();
        }
        [TestMethod]
        public void QuandoReceberUmIvaNovoDeveAdicionar()
        {
            Assert.Fail();
        }
    }
}
