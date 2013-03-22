using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;

namespace BsBios.Portal.Tests.Application.Services
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
        private readonly IList<CondicaoDePagamento> _condicoesRepositorio;
        public CadastroCondicaoPagamentoTests()
        {
            _condicoesRepositorio = new List<CondicaoDePagamento>();
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _condicoesDePagamentoMock = new Mock<ICondicoesDePagamento>(MockBehavior.Strict);
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>()))
                .Callback((CondicaoDePagamento condicaoDePagamento) =>
                    {
                        Assert.IsNotNull(condicaoDePagamento);
                        //callback assegura que a transação foi iniciada  e não foi fechada antes de salvar
                        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                    });
            _condicoesDePagamentoMock.Setup(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>()))
                //callback assegura que a transação foi iniciada e não foi fechada antes de consultar
                .Callback((string[] codigos) =>
                    {
                        _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                        _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
                        if (codigos.Contains("C001"))
                        {
                            _condicoesRepositorio.Add(new CondicaoDePagamentoParaAtualizacao("C001", "CONDICAO 001"));
                        }

                    })
                .Returns(_condicoesDePagamentoMock.Object);

            _condicoesDePagamentoMock.Setup(x => x.List()).Returns(_condicoesRepositorio);

            _cadastroCondicaoPagamento = new CadastroCondicaoPagamento(_unitOfWorkMock.Object, _condicoesDePagamentoMock.Object);
            _condicaoPagamento01 = new CondicaoDePagamentoCadastroVm()
                {
                    Codigo = "C001",
                    Descricao = "CONDICAO 001" 
                };

            _condicaoPagamento02 = new CondicaoDePagamentoCadastroVm()
                {
                    Codigo = "C002",
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
            _condicoesDePagamentoMock.Verify(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
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
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>())).Callback((
                CondicaoDePagamento condicaoDePagamento) =>
                {
                    Assert.IsNotNull(condicaoDePagamento);
                    Assert.IsInstanceOfType(condicaoDePagamento, typeof(CondicaoDePagamentoParaAtualizacao));
                    Assert.AreEqual("C001",condicaoDePagamento.Codigo);
                    Assert.AreEqual("CONDICAO 001 ALTERADA", condicaoDePagamento.Descricao);
                });

            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>()
                {
                    new CondicaoDePagamentoCadastroVm()
                    {
                        Codigo = "C001",
                        Descricao = "CONDICAO 001 ALTERADA" 
                    }
                });
        }

        [TestMethod]
        public void QuandoReceberUmaCondicaoDePagamentoNovaDeveAdicionar()
        {
            _condicoesDePagamentoMock.Setup(x => x.Save(It.IsAny<CondicaoDePagamento>())).Callback((
                CondicaoDePagamento condicaoDePagamento) =>
            {
                Assert.IsNotNull(condicaoDePagamento);
                Assert.IsNotInstanceOfType(condicaoDePagamento, typeof(CondicaoDePagamentoParaAtualizacao));
                Assert.AreEqual("C002", condicaoDePagamento.Codigo);
                Assert.AreEqual("CONDICAO 002", condicaoDePagamento.Descricao);
            });

            _cadastroCondicaoPagamento.AtualizarCondicoesDePagamento(new List<CondicaoDePagamentoCadastroVm>() { _condicaoPagamento02 });
        }


    }

    public class CondicaoDePagamentoParaAtualizacao: CondicaoDePagamento
    {
        public CondicaoDePagamentoParaAtualizacao(string codigo, string descricao) : base(codigo, descricao)
        {
        }
    }
}
