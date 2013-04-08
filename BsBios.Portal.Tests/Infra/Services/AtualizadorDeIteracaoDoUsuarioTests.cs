using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
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

        [TestMethod]
        public void QuandoAdicionaIteracaoDeUsuarioOcorrePersistencia()
        {
            ProcessoDeCotacao processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            IList<FornecedorParticipante> fornecedoresParticipantes = new List<FornecedorParticipante>();
            fornecedoresParticipantes.Add(processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao()));
            fornecedoresParticipantes.Add(processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao()));

            _atualizadorDeIteracaoDoUsuario.Adicionar(fornecedoresParticipantes);

            _processoCotacaoIteracoesUsuarioMock.Verify(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()), Times.Exactly(fornecedoresParticipantes.Count));
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
            
        }

        [TestMethod]
        public void QuandoOcorreErroAoAdicionarIteracaoDeUsuarioFazRollback()
        {
            _processoCotacaoIteracoesUsuarioMock.Setup(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()))
                                                .Throws(new ExcecaoDeTeste("Erro"));

            IList<FornecedorParticipante> fornecedoresParticipantes = new List<FornecedorParticipante>();

            try
            {
                ProcessoDeCotacao processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
                fornecedoresParticipantes.Add(processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao()));
                fornecedoresParticipantes.Add(processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao()));

                _atualizadorDeIteracaoDoUsuario.Adicionar(fornecedoresParticipantes);

                Assert.Fail("Deveria ter gerado exceção.");

            }
            catch (ExcecaoDeTeste)
            {
                _processoCotacaoIteracoesUsuarioMock.Verify(x => x.Save(It.IsAny<ProcessoCotacaoIteracaoUsuario>()), Times.Once());
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }

        }
    }
}
