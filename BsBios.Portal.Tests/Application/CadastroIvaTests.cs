using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Contracts;
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
        private readonly Mock<ICadastroIvaOperacao> _cadastroIvaOperacaoMock;
        private readonly ICadastroIva _cadastroIva;
        private readonly IvaCadastroVm _ivaPadrao;
        private readonly IList<IvaCadastroVm> _listaIvas;
        private readonly Mock<Iva> _ivaMock; 


        public CadastroIvaTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _ivasMock = new Mock<IIvas>(MockBehavior.Strict);
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>())).Callback((Iva iva) => Assert.IsNotNull(iva));
            _ivasMock.Setup(x => x.BuscaPeloCodigoSap(It.IsAny<string>()))
                     .Returns((string i) => i == "01" ? new Iva("01", "IVA 01") : null);

            _cadastroIvaOperacaoMock = new Mock<ICadastroIvaOperacao>(MockBehavior.Strict);
            _cadastroIvaOperacaoMock.Setup(x => x.Criar(It.IsAny<IvaCadastroVm>()))
                                    .Returns(new Iva("02", "IVA 02"));
            _cadastroIvaOperacaoMock.Setup(x => x.Alterar(It.IsAny<Iva>(), It.IsAny<IvaCadastroVm>()));
            _cadastroIva = new CadastroIva(_unitOfWorkMock.Object, _ivasMock.Object, _cadastroIvaOperacaoMock.Object);
            _ivaPadrao = new IvaCadastroVm()
                {
                    Codigo = "01",
                    Descricao = "IVA 01"
                };
            _listaIvas = new List<IvaCadastroVm>(){_ivaPadrao};
            _ivaMock = new Mock<Iva>(MockBehavior.Strict);
            _ivaMock.Setup(x => x.AtualizaDescricao(It.IsAny<string>()));
        }

        [TestMethod]
        public void QuandoCadastraUmNovoIvaOcorrePersistencia()
        {
            _cadastroIva.AtualizarIvas(_listaIvas);
            _ivasMock.Verify(x => x.Save(It.IsAny<Iva>()),Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmNovoIvaComSucessoFazCommitNaTransacao()
        {
            _cadastroIva.AtualizarIvas(_listaIvas);
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
                _cadastroIva.AtualizarIvas(_listaIvas);
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
            _cadastroIva.AtualizarIvas(_listaIvas);
            _cadastroIvaOperacaoMock.Verify(x => x.Alterar(It.IsAny<Iva>(), It.IsAny<IvaCadastroVm>()), Times.Once());
            _cadastroIvaOperacaoMock.Verify(x => x.Criar(It.IsAny<IvaCadastroVm>()),Times.Never());
        }
        [TestMethod]
        public void QuandoReceberUmIvaNovoDeveAdicionar()
        {
            _cadastroIva.AtualizarIvas(new List<IvaCadastroVm>(){new IvaCadastroVm(){Codigo = "02", Descricao = "IVA 02"}});
            _cadastroIvaOperacaoMock.Verify(x => x.Alterar(It.IsAny<Iva>(), It.IsAny<IvaCadastroVm>()), Times.Never());
            _cadastroIvaOperacaoMock.Verify(x => x.Criar(It.IsAny<IvaCadastroVm>()), Times.Once());
        }
    }
}
