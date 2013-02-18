using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
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
    public class CadastroCondicaoPagamentoTests
    {
        private readonly ICadastroCondicaoPagamento _cadastroCondicaoPagamento;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICondicoesDePagamento> _condicoesDePagamentoMock;
        private readonly CondicaoDePagamentoCadastroVm _condicaoPagamentoPadrao;

        public CadastroCondicaoPagamentoTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _condicoesDePagamentoMock = new Mock<ICondicoesDePagamento>(MockBehavior.Strict);
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>()));
            _cadastroCondicaoPagamento = new CadastroCondicaoPagamento(_unitOfWorkMock.Object, _condicoesDePagamentoMock.Object);
            _condicaoPagamentoPadrao = new CondicaoDePagamentoCadastroVm()
                {
                    CodigoSap = "C001",
                    Descricao = "CONDICAO 001" 
                };
        }

        [TestMethod]
        public void QuandoCadastroUmaNovaCondicaoDePagamentoOcorrePersistencia()
        {
            _cadastroCondicaoPagamento.Novo(_condicaoPagamentoPadrao);
            _condicoesDePagamentoMock.Verify(x => x.Save(It.IsAny<CondicaoDePagamento>()), Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmaNovaCondicaoDePagamentoComSucessoFazCommitNaTransacao()
        {
            _cadastroCondicaoPagamento.Novo(_condicaoPagamentoPadrao);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoFazRollbackNaTransacao()
        {
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar a condição de pagamento"));
            try
            {
                _cadastroCondicaoPagamento.Novo(_condicaoPagamentoPadrao);
                Assert.Fail("Deveria ter gerado excessão");
                
            }
            catch(ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
            }
        }

        [TestMethod]
        public void QuandoReceberUmaCondicaoDePagamentoExistenteDeveAtualizar()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoReceberUmaCondicaoDePagamentoNovaDeveAdicionar()
        {
            Assert.Fail();
        }
    }
}
