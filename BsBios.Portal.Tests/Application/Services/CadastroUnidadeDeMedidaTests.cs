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
    public class CadastroUnidadeDeMedidaTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IUnidadesDeMedida> _unidadesDeMedidaMock;
        private readonly ICadastroUnidadeDeMedida _cadastroUnidadeDeMedida;
        private readonly UnidadeDeMedidaCadastroVm _unidadeDeMedidaPadrao;
        private readonly IList<UnidadeDeMedidaCadastroVm> _listaunidadesDeMedida;
        private readonly IList<UnidadeDeMedida> _unidadesConsulta;

        public CadastroUnidadeDeMedidaTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _unidadesConsulta = new List<UnidadeDeMedida>();
            _unidadesDeMedidaMock = new Mock<IUnidadesDeMedida>(MockBehavior.Strict);
            _unidadesDeMedidaMock.Setup(x => x.Save(It.IsAny<UnidadeDeMedida>()))
                .Callback(CommonGenericMocks<UnidadeDeMedida>.DefaultSaveCallBack(_unitOfWorkMock));
            _unidadesDeMedidaMock.Setup(x => x.FiltraPorListaDeCodigosInternos(It.IsAny<string[]> ()))
                            .Returns(_unidadesDeMedidaMock.Object)
                            .Callback(
                                (string[] i) => 
                                {
                                    if (i.Contains("I01"))
                                    {
                                        _unidadesConsulta.Add(new UnidadeDeMedidaParaAtualizacao("I01", "E01", "Unidade 01"));
                                    }
                                });
           

            _unidadesDeMedidaMock.Setup(x => x.List())
                            .Returns(() => _unidadesConsulta );

            _cadastroUnidadeDeMedida = new CadastroUnidadeDeMedida(_unitOfWorkMock.Object, _unidadesDeMedidaMock.Object);
            _unidadeDeMedidaPadrao = new UnidadeDeMedidaCadastroVm()
                {
                    CodigoInterno = "I01",
                    CodigoExterno = "E01",
                    Descricao = "Unidade 01"
                };
            _listaunidadesDeMedida = new List<UnidadeDeMedidaCadastroVm>() { _unidadeDeMedidaPadrao };
        }

        [TestMethod]
        public void QuandoCadastraUmNovoIvaOcorrePersistencia()
        {
            _cadastroUnidadeDeMedida.AtualizarUnidadesDeMedida(_listaunidadesDeMedida);

            _unidadesDeMedidaMock.Verify(x => x.Save(It.IsAny<UnidadeDeMedida>()),Times.Once());
            _unidadesDeMedidaMock.Verify(x => x.FiltraPorListaDeCodigosInternos(It.IsAny<string[]>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);

        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoNoCadastroDoIvaFazRollBackNaTransacao()
        {
            _unidadesDeMedidaMock.Setup(x => x.Save(It.IsAny<UnidadeDeMedida>()))
                     .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar a Unidade de Medida"));

            try
            {
                _cadastroUnidadeDeMedida.AtualizarUnidadesDeMedida(_listaunidadesDeMedida);
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
            _unidadesDeMedidaMock.Setup(x => x.Save(It.IsAny<UnidadeDeMedida>())).Callback((UnidadeDeMedida unidadeDeMedida) =>
            {
                Assert.IsNotNull(unidadeDeMedida);
                Assert.IsInstanceOfType(unidadeDeMedida, typeof(UnidadeDeMedidaParaAtualizacao));
                //próximos asserts garantem que a atualização das propriedades foi feita corretamente dentro do método de atualização
                Assert.AreEqual("I01", unidadeDeMedida.CodigoInterno);
                Assert.AreEqual("E01", unidadeDeMedida.CodigoExterno);
                Assert.AreEqual("UNIDADE 01 ALTERADA", unidadeDeMedida.Descricao);
            });
            _cadastroUnidadeDeMedida.AtualizarUnidadesDeMedida(new List<UnidadeDeMedidaCadastroVm>()
                {
                    new UnidadeDeMedidaCadastroVm()
                        {
                            CodigoInterno = "I01",
                            CodigoExterno = "E01",
                            Descricao = "UNIDADE 01 ALTERADA"
                        }
                });
        }
        [TestMethod]
        public void QuandoReceberUmIvaNovoDeveAdicionar()
        {
            _unidadesDeMedidaMock.Setup(x => x.Save(It.IsAny<UnidadeDeMedida>())).Callback((UnidadeDeMedida unidadeDeMedida) =>
            {
                //garanto que foi passada um objeto instancia para o método Save
                Assert.IsNotNull(unidadeDeMedida);
                //Garanto que que a instância da classe utilizada no momento de salvar o Iva não é do tipo IvaParaAtualizacao,
                //que é utilizada apenas no update
                Assert.IsNotInstanceOfType(unidadeDeMedida, typeof(UnidadeDeMedidaParaAtualizacao));
                //próximos asserts garantem que a criação do objeto foi feita corretamente dentro do método de criação
                Assert.AreEqual("I02", unidadeDeMedida.CodigoInterno);
                Assert.AreEqual("E02", unidadeDeMedida.CodigoExterno);
                Assert.AreEqual("UNIDADE 02", unidadeDeMedida.Descricao);
            });

            _cadastroUnidadeDeMedida.AtualizarUnidadesDeMedida(new List<UnidadeDeMedidaCadastroVm>() 
            { new UnidadeDeMedidaCadastroVm() { CodigoInterno = "I02", CodigoExterno = "E02", Descricao = "UNIDADE 02" } });
        }
    }

    //ItinerarioParaAtualizacao é o tipo criado para ser utilizado na operação de atualização de cadastro e poder diferir da classe utilizada para inserção
    public class UnidadeDeMedidaParaAtualizacao: UnidadeDeMedida
    {
        public UnidadeDeMedidaParaAtualizacao(string codigoInterno, string codigoExterno, string descricao)
            : base(codigoInterno, codigoExterno, descricao)
        {
        }
    }
}
