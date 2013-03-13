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
using System.Linq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroIvaTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IIvas> _ivasMock;
        private readonly ICadastroIva _cadastroIva;
        private readonly IvaCadastroVm _ivaPadrao;
        private readonly IList<IvaCadastroVm> _listaIvas;
        private readonly IList<Iva> _ivasConsulta;


        public CadastroIvaTests()
        {
            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();
            _ivasConsulta = new List<Iva>();
            _ivasMock = new Mock<IIvas>(MockBehavior.Strict);
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>())).Callback((Iva iva) => Assert.IsNotNull(iva));
            _ivasMock.Setup(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()))
                    .Callback((string[] codigos) =>
                        {
                            if (codigos.Contains("01"))
                            {
                                _ivasConsulta.Add(new IvaParaAtualizacao("01", "IVA 01"));
                            }
                            
                        })
                     .Returns(_ivasMock.Object);

            _ivasMock.Setup(x => x.List()).Returns(_ivasConsulta);

            _cadastroIva = new CadastroIva(_unitOfWorkMock.Object, _ivasMock.Object);
            _ivaPadrao = new IvaCadastroVm()
                {
                    Codigo = "01",
                    Descricao = "IVA 01"
                };
            _listaIvas = new List<IvaCadastroVm>(){_ivaPadrao};
        }

        [TestMethod]
        public void QuandoCadastraUmNovoIvaOcorrePersistencia()
        {
            _cadastroIva.AtualizarIvas(_listaIvas);
            _ivasMock.Verify(x => x.Save(It.IsAny<Iva>()),Times.Once());
            _ivasMock.Verify(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
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
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoReceberUmIvaExistenteDeveAtualizar()
        {
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>())).Callback((Iva iva) =>
            {
                Assert.IsNotNull(iva);
                Assert.IsInstanceOfType(iva, typeof(IvaParaAtualizacao));
                //próximos asserts garantem que a atualização das propriedades foi feita corretamente dentro do método de atualização
                Assert.AreEqual("01", iva.Codigo);
                Assert.AreEqual("IVA 01 ALTERADO", iva.Descricao);
            });               
            _cadastroIva.AtualizarIvas(new List<IvaCadastroVm>()
                {
                    new IvaCadastroVm()
                        {
                            Codigo = "01",
                            Descricao = "IVA 01 ALTERADO"
                        }
                });
        }
        [TestMethod]
        public void QuandoReceberUmIvaNovoDeveAdicionar()
        {
            _ivasMock.Setup(x => x.Save(It.IsAny<Iva>())).Callback((Iva iva) =>
            {
                //garanto que foi passada um objeto instancia para o método Save
                Assert.IsNotNull(iva);
                //Garanto que que a instância da classe utilizada no momento de salvar o Iva não é do tipo IvaParaAtualizacao,
                //que é utilizada apenas no update
                Assert.IsNotInstanceOfType(iva, typeof(IvaParaAtualizacao));
                //próximos asserts garantem que a criação do objeto foi feita corretamente dentro do método de criação
                Assert.AreEqual("02", iva.Codigo);
                Assert.AreEqual("IVA 02", iva.Descricao);
            });               

            _cadastroIva.AtualizarIvas(new List<IvaCadastroVm>(){new IvaCadastroVm(){Codigo = "02", Descricao = "IVA 02"}});
        }
    }

    //IvaParaAtualizacao é o tipo criado para ser utilizado na operação de atualização de cadastro e poder diferir da classe utilizada para inserção
    public class IvaParaAtualizacao: Iva
    {
        public IvaParaAtualizacao(string codigo, string descricao) : base(codigo, descricao)
        {
        }
    }
}
