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
    public class CadastroCondicaoPagamentoTests
    {
        private readonly ICadastroCondicaoPagamento _cadastroCondicaoPagamento;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICondicoesDePagamento> _condicoesDePagamentoMock;
        private readonly CondicaoDePagamentoCadastroVm _condicaoPagamento01;
        private readonly CondicaoDePagamentoCadastroVm _condicaoPagamento02;
        private readonly IList<CondicaoDePagamentoCadastroVm> _condicoesDePagamento;
        private readonly Mock<ICadastroCondicaoPagamentoOperacao> _cadastroCondicaoPagamentoOperacaoMock;
        public CadastroCondicaoPagamentoTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _condicoesDePagamentoMock = new Mock<ICondicoesDePagamento>(MockBehavior.Strict);
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>()));
            _condicoesDePagamentoMock.Setup(x => x.BuscaPeloCodigoSap(It.IsAny<string>()))
                                     .Returns((string c) => c == "C001" ? new CondicaoDePagamento("C001", "CONDICAO 001") : null);

            _cadastroCondicaoPagamentoOperacaoMock = new Mock<ICadastroCondicaoPagamentoOperacao>();

            _cadastroCondicaoPagamento = new CadastroCondicaoPagamento(_unitOfWorkMock.Object, _condicoesDePagamentoMock.Object, _cadastroCondicaoPagamentoOperacaoMock.Object);
            _condicaoPagamento01 = new CondicaoDePagamentoCadastroVm()
                {
                    CodigoSap = "C001",
                    Descricao = "CONDICAO 001" 
                };

            _condicaoPagamento02 = new CondicaoDePagamentoCadastroVm()
                {
                    CodigoSap = "C002",
                    Descricao = "CONDICAO 002"
                };

            _condicoesDePagamento = new List<CondicaoDePagamentoCadastroVm>()
                {
                    _condicaoPagamento01,_condicaoPagamento02
                };
            
        }

        [TestMethod]
        public void QuandoCadastroUmaNovaCondicaoDePagamentoOcorrePersistencia()
        {
            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>(){_condicaoPagamento01});
            _condicoesDePagamentoMock.Verify(x => x.Save(It.IsAny<CondicaoDePagamento>()), Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmaNovaCondicaoDePagamentoComSucessoFazCommitNaTransacao()
        {
            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>() { _condicaoPagamento01 });
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoFazRollbackNaTransacao()
        {
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>())).Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar a condição de pagamento"));
            try
            {
                _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(_condicoesDePagamento);
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
            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>(){_condicaoPagamento01});
            _cadastroCondicaoPagamentoOperacaoMock.Verify(x => x.Alterar(It.IsAny<CondicaoDePagamento>(), It.IsAny<CondicaoDePagamentoCadastroVm>()), Times.Once());
            _cadastroCondicaoPagamentoOperacaoMock.Verify(x => x.Criar(It.IsAny<CondicaoDePagamentoCadastroVm>()),Times.Never());
        }

        [TestMethod]
        public void QuandoReceberUmaCondicaoDePagamentoNovaDeveAdicionar()
        {
            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>() { _condicaoPagamento02 });
            _cadastroCondicaoPagamentoOperacaoMock.Verify(x => x.Alterar(It.IsAny<CondicaoDePagamento>(), It.IsAny<CondicaoDePagamentoCadastroVm>()), Times.Never());
            _cadastroCondicaoPagamentoOperacaoMock.Verify(x => x.Criar(It.IsAny<CondicaoDePagamentoCadastroVm>()), Times.Once());
        }
    }
}
