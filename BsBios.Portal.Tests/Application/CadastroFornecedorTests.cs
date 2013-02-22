using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
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
    public class CadastroFornecedorTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFornecedores> _fornecedoresMock; 
        private readonly FornecedorCadastroVm _fornecedorCadastroVm;
        private readonly ICadastroFornecedor _cadastroFornecedor;
        private readonly Mock<Fornecedor> _fornecedorMock;
        private readonly Mock<ICadastroFornecedorOperacao> _cadastroFornecedorOperacao;
         
        public CadastroFornecedorTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _fornecedorMock = new Mock<Fornecedor>(MockBehavior.Default);
            _fornecedorMock.Setup(x => x.Atualizar(It.IsAny<string>(), It.IsAny<string>()));

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>())).Callback((Fornecedor fornecedor) => Assert.IsNotNull(fornecedor));
            _fornecedoresMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>())).Returns((string f) => f == "FORNEC0001" ? _fornecedorMock.Object : null);

            _cadastroFornecedorOperacao = new Mock<ICadastroFornecedorOperacao>(MockBehavior.Strict);
            _cadastroFornecedorOperacao.Setup(x => x.Criar(It.IsAny<FornecedorCadastroVm>()))
                .Returns(new Fornecedor("FORNEC002", "FORNECEDOR 0002", "fornecedor2@empresa.com.br"));
            _cadastroFornecedorOperacao.Setup(x => x.Atualizar(It.IsAny<Fornecedor>(), It.IsAny<FornecedorCadastroVm>()));

            _cadastroFornecedor = new CadastroFornecedor(_unitOfWorkMock.Object, _fornecedoresMock.Object, _cadastroFornecedorOperacao.Object);

            _fornecedorCadastroVm = new FornecedorCadastroVm()
                {
                    Codigo = "FORNEC0001",
                    Nome = "FORNECEDOR 0001",
                    Email = "fornecedor@empresa.com.br"
                };
        }

        [TestMethod]
        public void QuandoCadastroUmNovoFornecedorERealizadaPersistencia()
        {
            _cadastroFornecedor.Novo(_fornecedorCadastroVm);
            _fornecedoresMock.Verify(x => x.Save(It.IsAny<Fornecedor>()),Times.Once());
        }
        [TestMethod]
        public void QuandoCadastroUmNovoFornecedorComSucessoERealizadoCommit()
        {
            _cadastroFornecedor.Novo(_fornecedorCadastroVm);
            _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
            _unitOfWorkMock.Verify(x => x.Commit(),Times.Once());
            _unitOfWorkMock.Verify(x =>x.RollBack(), Times.Never());
        }
        [TestMethod]
        public void QuandoOcorreAlgumaExcecaoEhRealizadoRollback()
        {
            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>()))
                             .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o Fornecedor"));

            try
            {
                _cadastroFornecedor.Novo(_fornecedorCadastroVm);
                Assert.Fail("Deveria ter gerado excessão");
            }
            catch (ExcecaoDeTeste)
            {
                _unitOfWorkMock.Verify(x => x.BeginTransaction(), Times.Once());
                _unitOfWorkMock.Verify(x => x.RollBack(), Times.Once());
                _unitOfWorkMock.Verify(x => x.Commit(), Times.Never());
            }
        }

        [TestMethod]
        public void QuandoReceberUmFornecedorExistenteDeveAtualizar()
        {
            _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            Codigo ="FORNEC0001" ,
                            Nome = "FORNECEDOR 0001 ATUALIZADO" ,
                            Email = "emailatualizado@empresa.com.br"
                        }
                });

            _cadastroFornecedorOperacao.Verify(x => x.Criar(It.IsAny<FornecedorCadastroVm>()), Times.Never());
            _cadastroFornecedorOperacao.Verify(x => x.Atualizar(It.IsAny<Fornecedor>(), It.IsAny<FornecedorCadastroVm>()), Times.Once());

        }
        [TestMethod]
        public void QuandoReceberUmFornecedorNovoDeveAdicionar()
        {
            _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            Codigo = "FORNEC0002" ,
                            Nome =  "FORNECEDOR 0002",
                            Email = "fornecedor0002@empresa.com.br"
                        }
                });

            _cadastroFornecedorOperacao.Verify(x => x.Criar(It.IsAny<FornecedorCadastroVm>()), Times.Once());
            _cadastroFornecedorOperacao.Verify(x => x.Atualizar(It.IsAny<Fornecedor>(), It.IsAny<FornecedorCadastroVm>()),Times.Never());

        }

    }

}
