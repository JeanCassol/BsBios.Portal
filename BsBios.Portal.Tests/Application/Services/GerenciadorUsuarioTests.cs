using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
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
        private readonly Mock<IGeradorDeEmail> _geradorDeEmailMock;

        public GerenciadorUsuarioTests()
        {
            _provedorDeCriptografiaMock = new Mock<IProvedorDeCriptografia>(MockBehavior.Strict);
            _provedorDeCriptografiaMock.Setup(x => x.Criptografar(It.IsAny<string>())).Returns("criptografado");

            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _usuariosMock = new Mock<IUsuarios>(MockBehavior.Strict);

            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                .Callback(CommonGenericMocks<Usuario>.DefaultSaveCallBack(_unitOfWorkMock));


            _usuariosMock.Setup(x => x.BuscaPorLogin(It.IsAny<string>()))
                         .Returns(
                             (string login) =>
                             login == "USER001"
                                 ? new UsuarioParaAtualizacao("USUARIO 001", "USER001", "")
                                  : null);

            _usuariosMock.Setup(x => x.FiltraPorListaDeLogins(It.IsAny<string[]>())).Returns(_usuariosMock.Object);
            _usuariosMock.Setup(x => x.SemSenha()).Returns(_usuariosMock.Object);

            _geradorDeSenhaMock = new Mock<IGeradorDeSenha>(MockBehavior.Strict);
            _geradorDeSenhaMock.Setup(x => x.GerarGuid(It.IsAny<int>()))
                               .Returns("12345678");

            
            _geradorDeEmailMock = new Mock<IGeradorDeEmail>(MockBehavior.Strict);
            _geradorDeEmailMock.Setup(x => x.CriacaoAutomaticaDeSenha(It.IsAny<Usuario>(), It.IsAny<string>()));
            _gerenciadorUsuario = new GerenciadorUsuario(_unitOfWorkMock.Object,_usuariosMock.Object, 
                _provedorDeCriptografiaMock.Object, _geradorDeSenhaMock.Object,
                ObjectFactory.GetInstance<IBuilder<Usuario, UsuarioConsultaVm>>(),_geradorDeEmailMock.Object);

        }

        [TestMethod]
        public void QuandoAlterarPerfisDoUsuarioOMesmoContemOsNovosPerfis()
        {
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()))
                         .Callback((Usuario usuario) =>
                             {
                                 Assert.IsNotNull(usuario);
                                 Assert.AreEqual(2, usuario.Perfis.Count);
                                 Assert.IsTrue(usuario.Perfis.Contains(Enumeradores.Perfil.CompradorLogistica));
                                 Assert.IsTrue(usuario.Perfis.Contains(Enumeradores.Perfil.CompradorSuprimentos));
                             });
            _gerenciadorUsuario.AtualizarPerfis("USER001",new List<Enumeradores.Perfil>
                {
                    Enumeradores.Perfil.CompradorLogistica,
                    Enumeradores.Perfil.CompradorSuprimentos
                });

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

        [TestMethod]
        public void QuandoCrioUmaNovaSenhaEEnviadoEmailDeConfirmacaoParaUsuario()
        {
            _gerenciadorUsuario.CriarSenha("USER001");
            _geradorDeEmailMock.Verify(x => x.CriacaoAutomaticaDeSenha(It.IsAny<Usuario>(),It.IsAny<string>()),Times.Once());
        }

        #endregion

        #region Criar senha para usuários sem senha
        [TestMethod]
        public void CriarSenhasParaUsuarioSemSenhaGeraEmailESenhaParaUsuariosSemSenha()
        {
            _usuariosMock.Setup(x => x.Save(It.IsAny<Usuario>()));
            _usuariosMock.Setup(x => x.List())
                         .Returns(new List<Usuario>
                             {
                                 DefaultObjects.ObtemUsuarioPadrao(),
                                 DefaultObjects.ObtemUsuarioPadrao()
                             });

            _gerenciadorUsuario.CriarSenhaParaUsuariosSemSenha(new []{"0001","0002"});

            _usuariosMock.Verify(x => x.FiltraPorListaDeLogins(It.IsAny<string[]>()),Times.Once());
            _usuariosMock.Verify(x => x.SemSenha(),Times.Once());
            _usuariosMock.Verify( x=> x.Save(It.IsAny<Usuario>()), Times.Exactly(2));
            _geradorDeEmailMock.Verify(x => x.CriacaoAutomaticaDeSenha(It.IsAny<Usuario>(), It.IsAny<string>()),  Times.Exactly(2));
            _geradorDeSenhaMock.Verify(x => x.GerarGuid(It.IsAny<int>()), Times.Exactly(2));
            _provedorDeCriptografiaMock.Verify( x => x.Criptografar(It.IsAny<string>()), Times.Exactly(2));

        }
        #endregion

    }
}
