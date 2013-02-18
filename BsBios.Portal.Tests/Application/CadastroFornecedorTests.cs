using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public class CadastroFornecedorTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IFornecedores> _fornecedoresMock; 
        private readonly FornecedorCadastroVm _fornecedorCadastroVm;
        private readonly ICadastroFornecedor _cadastroFornecedor;
        private readonly Mock<Fornecedor> _fornecedorMock; 
         
        public CadastroFornecedorTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _fornecedorMock = new Mock<Fornecedor>(MockBehavior.Default);
            _fornecedorMock.Setup(x => x.Atualizar(It.IsAny<string>(), It.IsAny<string>()));

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.Save(It.IsAny<Fornecedor>()));
            _fornecedoresMock.Setup(x => x.BuscaPeloCodigoSap(It.IsAny<string>())).Returns((string f) => f == "FORNEC0001" ? _fornecedorMock.Object : null);

            _cadastroFornecedor = new CadastroFornecedor(_unitOfWorkMock.Object, _fornecedoresMock.Object);

            _fornecedorCadastroVm = new FornecedorCadastroVm()
                {
                    CodigoSap = "FORNEC0001",
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
            IList<Fornecedor> fornecedoresAtualizados = _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            CodigoSap ="FORNEC0001" ,
                            Nome = "FORNECEDOR 0001 ATUALIZADO" ,
                            Email = "emailatualizado@empresa.com.br"
                        }
                });
             _fornecedorMock.Verify(x => x.Atualizar(It.IsAny<string>(), It.IsAny<string>()), Times.Exactly(fornecedoresAtualizados.Count));

            //_fornecedorMock.SetupGet(x => x.Codigo).Returns("FORNEC0001");
            
            var fornecedorAtualizado = fornecedoresAtualizados.First();
            Assert.AreEqual("FORNEC0001", fornecedorAtualizado.Codigo);
            Assert.AreEqual("FORNECEDOR 0001 ATUALIZADO", fornecedorAtualizado.Nome);
            Assert.AreEqual("emailatualizado@empresa.com.br",fornecedorAtualizado.Email);

        }
        [TestMethod]
        public void QuandoReceberUmFornecedorNovoDeveAdicionar()
        {
            IList<Fornecedor> fornecedoresAtualizados = _cadastroFornecedor.AtualizarFornecedores(new List<FornecedorCadastroVm>()
                {
                    new FornecedorCadastroVm()
                        {
                            CodigoSap = "FORNEC0002" ,
                            Nome =  "FORNECEDOR 0002",
                            Email = "fornecedor0002@empresa.com.br"
                        }
                });
            _fornecedorMock.Verify(x => x.Atualizar(It.IsAny<string>(), It.IsAny<string>()), Times.Never());

            var fornecedorAtualizado = fornecedoresAtualizados.First();
            Assert.AreEqual("FORNEC0002", fornecedorAtualizado.Codigo);
            Assert.AreEqual("FORNECEDOR 0002", fornecedorAtualizado.Nome);
            Assert.AreEqual("fornecedor0002@empresa.com.br",fornecedorAtualizado.Email);
        }

    }

}
