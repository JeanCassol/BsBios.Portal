using System;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using BsBios.Portal.Tests.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Infra.Services
{
    [TestClass]
    public class AtualizadorDeIteracaoDoUsuarioTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessoCotacaoIteracoesUsuario> _processoCotacaoIteracoesUsuarioMock;
        private readonly IAtualizadorDeIteracaoDoUsuario _atualizadorDeIteracaoDoUsuario;

        public AtualizadorDeIteracaoDoUsuarioTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();
            _processoCotacaoIteracoesUsuarioMock = new Mock<IProcessoCotacaoIteracoesUsuario>(MockBehavior.Strict);
            _processoCotacaoIteracoesUsuarioMock.Setup(x => x.BuscaPorIdParticipante(It.IsAny<int>()))
                                                .Returns(new ProcessoCotacaoIteracaoUsuario(10));

            _processoCotacaoIteracoesUsuarioMock.Setup(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()))
                                                .Callback(
                                                    CommonGenericMocks<ProcessoCotacaoIteracaoUsuario>
                                                        .DefaultSaveCallBack(_unitOfWorkMock));
            _atualizadorDeIteracaoDoUsuario = new AtualizadorDeIteracaoDoUsuario(_unitOfWorkMock.Object, _processoCotacaoIteracoesUsuarioMock.Object);
        }

        [TestMethod]
        public void QuandoAtualizaComSucessoOcorrePersistencia()
        {
            _atualizadorDeIteracaoDoUsuario.Atualizar(10);
            _processoCotacaoIteracoesUsuarioMock.Verify(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreErroAoAtualizarOcorreRollback()
        {
            _processoCotacaoIteracoesUsuarioMock.Setup(x => x.BuscaPorIdParticipante(It.IsAny<int>()))
                                                .Throws(new ExcecaoDeTeste("Ocorreu erro ao consultar a iteração do usuário"));

            try
            {
                _atualizadorDeIteracaoDoUsuario.Atualizar(10);

            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        public void QuandoAtualizaIteracaoFicaVisualizada()
        {
            _processoCotacaoIteracoesUsuarioMock.Setup(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()))
                                                .Callback((ProcessoCotacaoIteracaoUsuario p) =>
                                                    {
                                                        Assert.IsNotNull(p);
                                                        Assert.IsTrue(p.VisualizadoPeloFornecedor);

                                                    });
            _atualizadorDeIteracaoDoUsuario.Atualizar(10);
        }
    }
}
