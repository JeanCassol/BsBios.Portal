using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroIncotermTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IIncoterms> _incotermsMock;
        private readonly ICadastroIncoterm _cadastroIncoterm;
        private readonly IncotermCadastroVm _incotermPadrao;
        private readonly IList<IncotermCadastroVm> _listaIncoterms;
        private Incoterm _incotermRetornoRepositorio;

        public CadastroIncotermTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _incotermsMock = new Mock<IIncoterms>(MockBehavior.Strict);
            _incotermsMock.Setup(x => x.Save(It.IsAny<Incoterm>())).Callback((Incoterm incoterm) => Assert.IsNotNull(incoterm));
            _incotermsMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                .Callback((string i) =>
                    {
                        _incotermRetornoRepositorio = (i == "001"
                                                           ? new IncotermParaAtualizacao("001", "INCOTERM 001")
                                                           : null);
                    })
                     .Returns(_incotermsMock.Object);

            _incotermsMock.Setup(x => x.Single())
                          .Returns(() =>  _incotermRetornoRepositorio);

            _cadastroIncoterm = new CadastroIncoterm(_unitOfWorkMock.Object, _incotermsMock.Object);
            _incotermPadrao = new IncotermCadastroVm()
                {
                    Codigo = "001",
                    Descricao = "INCOTERM 001"
                };
            _listaIncoterms = new List<IncotermCadastroVm>(){_incotermPadrao};
        }

        [TestMethod]
        public void QuandoCadastraUmNovoIncotermOcorrePersistencia()
        {
            _cadastroIncoterm.AtualizarIncoterms(_listaIncoterms);
            _incotermsMock.Verify(x => x.Save(It.IsAny<Incoterm>()),Times.Once());
        }

        [TestMethod]
        public void QuandoCadastroUmNovoIncotermComSucessoFazCommitNaTransacao()
        {
            _cadastroIncoterm.AtualizarIncoterms(_listaIncoterms);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());       
            _unitOfWorkMock.Verify(x => x.Commit(), Times.Once());
            _unitOfWorkMock.Verify(x => x.RollBack(), Times.Never());
        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoNoCadastroDoIncotermFazRollBackNaTransacao()
        {
            _incotermsMock.Setup(x => x.Save(It.IsAny<Incoterm>()))
                     .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o Iva"));

            try
            {
                _cadastroIncoterm.AtualizarIncoterms(_listaIncoterms);
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
        public void QuandoReceberUmIncotermExistenteDeveAtualizar()
        {
            _incotermsMock.Setup(x => x.Save(It.IsAny<Incoterm>())).Callback((Incoterm incoterm) =>
            {
                Assert.IsNotNull(incoterm);
                Assert.IsInstanceOfType(incoterm, typeof(IncotermParaAtualizacao));
                //próximos asserts garantem que a atualização das propriedades foi feita corretamente dentro do método de atualização
                Assert.AreEqual("001", incoterm.Codigo);
                Assert.AreEqual("INCOTERM 001 ALTERADO", incoterm.Descricao);
            });
            _cadastroIncoterm.AtualizarIncoterms(new List<IncotermCadastroVm>()
                {
                    new IncotermCadastroVm()
                        {
                            Codigo = "001",
                            Descricao = "INCOTERM 001 ALTERADO"
                        }
                });
        }
        [TestMethod]
        public void QuandoReceberUmIncotermNovoDeveAdicionar()
        {
            _incotermsMock.Setup(x => x.Save(It.IsAny<Incoterm>())).Callback((Incoterm incoterm) =>
            {
                //garanto que foi passada um objeto instancia para o método Save
                Assert.IsNotNull(incoterm);
                //Garanto que que a instância da classe utilizada no momento de salvar o Iva não é do tipo IvaParaAtualizacao,
                //que é utilizada apenas no update
                Assert.IsNotInstanceOfType(incoterm, typeof(IncotermParaAtualizacao));
                //próximos asserts garantem que a criação do objeto foi feita corretamente dentro do método de criação
                Assert.AreEqual("002", incoterm.Codigo);
                Assert.AreEqual("INCOTERM 002", incoterm.Descricao);
            });

            _cadastroIncoterm.AtualizarIncoterms(new List<IncotermCadastroVm>() { new IncotermCadastroVm() { Codigo = "002", Descricao = "INCOTERM 002" } });
        }
    }

    //IncotermParaAtualizacao é o tipo criado para ser utilizado na operação de atualização de cadastro e poder diferir da classe utilizada para inserção
    public class IncotermParaAtualizacao: Incoterm
    {
        public IncotermParaAtualizacao(string codigo, string descricao) : base(codigo, descricao)
        {
        }
    }
}
