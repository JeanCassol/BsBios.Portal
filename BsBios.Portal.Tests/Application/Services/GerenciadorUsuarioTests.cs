﻿using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class GerenciadorUsuarioTests
    {
        private readonly IGerenciadorUsuario _gerenciadorUsuario;
        private readonly Mock<IProvedorDeCriptografia> _provedorDeCriptografiaMock;
        private readonly Mock<IUsuarios> _usuariosMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IGeradorDeSenha> _geradorDeSenhaMock;

        public GerenciadorUsuarioTests()
        {
            _provedorDeCriptografiaMock = new Mock<IProvedorDeCriptografia>(MockBehavior.Strict);
            _provedorDeCriptografiaMock.Setup(x => x.Criptografar(It.IsAny<string>())).Returns("criptografado");

            _unitOfWorkMock = DefaultRepository.GetDefaultMockUnitOfWork();

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);

            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                .Callback(CommonGenericMocks<Usuario>.DefaultSaveCallBack(_unitOfWorkMock));


            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>()))
                         .Returns(
                             (string login) =>
                             login == "USER001"
                                 ? new UsuarioParaAtualizacao("USUARIO 001", "USER001", "")
                                  : null);

            _geradorDeSenhaMock = new Mock<IGeradorDeSenha>(MockBehavior.Strict);
            _geradorDeSenhaMock.Setup(x => x.GerarGuid(It.IsAny<int>()))
                               .Returns("12345678");

            _gerenciadorUsuario = new GerenciadorUsuario(_unitOfWorkMock.Object,_usuariosMock.Object, 
                _provedorDeCriptografiaMock.Object, _geradorDeSenhaMock.Object,
                ObjectFactory.GetInstance<IBuilder<Usuario, UsuarioConsultaVm>>());

        }

        [TestMethod]
        public void QuandoAlterarPerfisDoUsuarioOMesmoApareceComOsNovosPerfis()
        {

        }

        #region criar senha
        [TestMethod]
        public void UmaNovaSenhaCriadaParaOUsarioECriptografada()
        {

            _gerenciadorUsuario.CriarSenha("USER001");
            _geradorDeSenhaMock.Verify(x => x.GerarGuid(It.IsAny<int>()), Times.Once());
            _provedorDeCriptografiaMock.Verify(x => x.Criptografar(It.IsAny<string>()), Times.Once());
        }
        [TestMethod]
        public void QuandoCrioSenhaParaUsuarioOcorrePersistencia()
        {
            _gerenciadorUsuario.CriarSenha("USER001");
            _usuariosMock.Verify(x => x.Save(It.IsAny<Usuario>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }
        [TestMethod]
        public void QuandoOcorreErroAoCriarSenhaOcorreRollbackNaTrasacao()
        {
            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>()))
                         .Throws(new ExcecaoDeTeste("Erro ao consultar usuário"));

            try
            {
                _gerenciadorUsuario.CriarSenha("USER001");
            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }

        }

        #endregion


    }
}
