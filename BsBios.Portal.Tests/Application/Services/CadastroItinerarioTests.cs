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
    public class CadastroItinerarioTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IItinerarios> _itinerariosMock;
        private readonly ICadastroItinerario _cadastroItinerario;
        private readonly ItinerarioCadastroVm _itinerarioPadrao;
        private readonly IList<ItinerarioCadastroVm> _listaItinerarios;
        private readonly IList<Itinerario> _itinerariosConsulta;

        public CadastroItinerarioTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _itinerariosConsulta = new List<Itinerario>();
            _itinerariosMock = new Mock<IItinerarios>(MockBehavior.Strict);
            _itinerariosMock.Setup(x => x.Save(It.IsAny<Itinerario>())).Callback(CommonGenericMocks<Itinerario>.DefaultSaveCallBack(_unitOfWorkMock));
            _itinerariosMock.Setup(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>    ()))
                            .Returns(_itinerariosMock.Object)
                            .Callback(
                                (string[] codigos) =>
                                    {
                                        if (codigos.Contains("01"))
                                        {
                                            _itinerariosConsulta.Add(new ItinerarioParaAtualizacao("01", "Itinerario 01"));
                                            
                                        }
                                    });
           

            _itinerariosMock.Setup(x => x.List())
                            .Returns(() => _itinerariosConsulta );

            _cadastroItinerario = new CadastroItinerario(_unitOfWorkMock.Object, _itinerariosMock.Object);
            _itinerarioPadrao = new ItinerarioCadastroVm()
                {
                    Codigo = "01",
                    Descricao = "ITINERARIO 01"
                };
            _listaItinerarios = new List<ItinerarioCadastroVm>(){_itinerarioPadrao};
        }

        [TestMethod]
        public void QuandoCadastraUmNovoItinerarioOcorrePersistencia()
        {
            _cadastroItinerario.AtualizarItinerarios(_listaItinerarios);

            _itinerariosMock.Verify(x => x.Save(It.IsAny<Itinerario>()),Times.Once());
            _itinerariosMock.Verify(x => x.FiltraPorListaDeCodigos(It.IsAny<string[]>()), Times.Once());

            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);

        }

        [TestMethod]
        public void QuandoOcorreAlgumExcecaoNoCadastroDoItinerarioFazRollBackNaTransacao()
        {
            _itinerariosMock.Setup(x => x.Save(It.IsAny<Itinerario>()))
                     .Throws(new ExcecaoDeTeste("Ocorreu um erro ao cadastrar o Iva"));

            try
            {
                _cadastroItinerario.AtualizarItinerarios(_listaItinerarios);
                Assert.Fail("Deveria ter gerado exceção");

            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoReceberUmItinerarioExistenteDeveAtualizar()
        {
            _itinerariosMock.Setup(x => x.Save(It.IsAny<Itinerario>())).Callback((Itinerario itinerario) =>
            {
                Assert.IsNotNull(itinerario);
                Assert.IsInstanceOfType(itinerario, typeof(ItinerarioParaAtualizacao));
                //próximos asserts garantem que a atualização das propriedades foi feita corretamente dentro do método de atualização
                Assert.AreEqual("01", itinerario.Codigo);
                Assert.AreEqual("ITINERARIO 01 ALTERADO", itinerario.Descricao);
            });
            _cadastroItinerario.AtualizarItinerarios(new List<ItinerarioCadastroVm>()
                {
                    new ItinerarioCadastroVm()
                        {
                            Codigo = "01",
                            Descricao = "ITINERARIO 01 ALTERADO"
                        }
                });
        }
        [TestMethod]
        public void QuandoReceberUmItinerarioNovoDeveAdicionar()
        {
            _itinerariosMock.Setup(x => x.Save(It.IsAny<Itinerario>())).Callback((Itinerario itinerario) =>
            {
                //garanto que foi passada um objeto instancia para o método Save
                Assert.IsNotNull(itinerario);
                //Garanto que que a instância da classe utilizada no momento de salvar o Iva não é do tipo IvaParaAtualizacao,
                //que é utilizada apenas no update
                Assert.IsNotInstanceOfType(itinerario, typeof(ItinerarioParaAtualizacao));
                //próximos asserts garantem que a criação do objeto foi feita corretamente dentro do método de criação
                Assert.AreEqual("02", itinerario.Codigo);
                Assert.AreEqual("ITINERARIO 02", itinerario.Descricao);
            });

            _cadastroItinerario.AtualizarItinerarios(new List<ItinerarioCadastroVm>() { new ItinerarioCadastroVm() { Codigo = "02", Descricao = "ITINERARIO 02" } });
        }
    }

    //ItinerarioParaAtualizacao é o tipo criado para ser utilizado na operação de atualização de cadastro e poder diferir da classe utilizada para inserção
    public class ItinerarioParaAtualizacao: Itinerario
    {
        public ItinerarioParaAtualizacao(string codigo, string descricao): base(codigo, descricao)
        {
        }
    }
}
