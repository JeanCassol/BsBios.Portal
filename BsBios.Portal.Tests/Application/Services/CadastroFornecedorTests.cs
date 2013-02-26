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
    public class CadastroFornecedorTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFornecedores> _fornecedoresMock; 
        private readonly FornecedorCadastroVm _fornecedorCadastroVm;
        private readonly ICadastroFornecedor _cadastroFornecedor;
         
        public CadastroFornecedorTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>())).Callback((Fornecedor fornecedor) => Assert.IsNotNull(fornecedor));
            _fornecedoresMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                .Returns((string f) => f == "FORNEC0001" ? new FornecedorParaAtualizacao("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br") : null);

            _cadastroFornecedor = new CadastroFornecedor(_unitOfWorkMock.Object, _fornecedoresMock.Object);

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

            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>())).Callback((Fornecedor fornecedor) =>
                {
                    Assert.IsNotNull(fornecedor);
                    Assert.IsInstanceOfType(fornecedor, typeof(FornecedorParaAtualizacao));
                    Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
                    Assert.AreEqual("FORNECEDOR 0001 ATUALIZADO", fornecedor.Nome);
                    Assert.AreEqual("emailatualizado@empresa.com.br",fornecedor.Email);
                });
            _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            Codigo ="FORNEC0001" ,
                            Nome = "FORNECEDOR 0001 ATUALIZADO" ,
                            Email = "emailatualizado@empresa.com.br"
                        }
                });

        }
        [TestMethod]
        public void QuandoReceberUmFornecedorNovoDeveAdicionar()
        {
            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>())).Callback((Fornecedor fornecedor) =>
            {
                Assert.IsNotNull(fornecedor);
                Assert.IsNotInstanceOfType(fornecedor, typeof(FornecedorParaAtualizacao));
                Assert.AreEqual("FORNEC0002", fornecedor.Codigo);
                Assert.AreEqual("FORNECEDOR 0002", fornecedor.Nome);
                Assert.AreEqual("fornecedor0002@empresa.com.br", fornecedor.Email);
            });

            _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            Codigo = "FORNEC0002" ,
                            Nome =  "FORNECEDOR 0002",
                            Email = "fornecedor0002@empresa.com.br"
                        }
                });

        }

    }

    public class FornecedorParaAtualizacao: Fornecedor
    {
        public FornecedorParaAtualizacao(string codigo, string nome, string email) : base(codigo, nome, email)
        {
        }
    }

}
