using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class CadastroQuotaTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IQuotas> _quotasMock;
        private readonly Mock<IFornecedores> _fornecedoresMock;
        private readonly ICadastroQuota _cadastroQuota;
        private readonly Fornecedor _fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
        private readonly Fornecedor _fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
        private readonly Fornecedor _fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();

        public CadastroQuotaTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _quotasMock = new Mock<IQuotas>(MockBehavior.Strict);
            _quotasMock.Setup(x => x.Save(It.IsAny<Quota>()))
                       .Callback(CommonGenericMocks<Quota>.DefaultSaveCallBack(_unitOfWorkMock));

            _quotasMock.Setup(x => x.Delete(It.IsAny<Quota>()))
                       .Callback((Quota quota) => Assert.AreSame(_fornecedor2, quota.Fornecedor));

            _quotasMock.Setup(x => x.FiltraPorData(It.IsAny<DateTime>()))
                       .Returns(_quotasMock.Object);
            _quotasMock.Setup(x => x.List()).Returns(new List<Quota>()
                {
                    new Quota(Enumeradores.MaterialDeCarga.Soja, _fornecedor1,"1000",DateTime.Today,100 ),
                    new Quota(Enumeradores.MaterialDeCarga.Soja, _fornecedor2,"1000",DateTime.Today,120 )
                });

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()))
                             .Returns(_fornecedoresMock.Object);
            _fornecedoresMock.Setup(x => x.List()).Returns(new List<Fornecedor>
                {
                    _fornecedor3
                });
            _cadastroQuota = new CadastroQuota(_unitOfWorkMock.Object, _quotasMock.Object, _fornecedoresMock.Object);
        }

        [TestMethod]
        public void QuandoCadastroQuotasComSucessoOcorrePersistencia()
        {
            //caso de uso: para a data de hoje tenho duas quotas cadastradas:uma para o fornecedor1 e outra para o fornecedor3
            //salvo as cotações informando os fornecedores 1 e 3. O resultado esperado é que a quota do fornecedor1 seja atualizada,
            //a quota do fornecedor2 seja atualizada e a quota do fornecedor seja criada.
            _cadastroQuota.Salvar(new List<QuotaSalvarVm>
                {
                    new QuotaSalvarVm
                        {
                            Data = DateTime.Today,
                            CodigoTerminal = "1000",
                            CodigoMaterial = (int) Enumeradores.MaterialDeCarga.Soja,
                            CodigoFornecedor = _fornecedor1.Codigo,
                            Peso = 100
                        },
                    new QuotaSalvarVm
                        {
                            Data = DateTime.Today,
                            CodigoTerminal = "1000",
                            CodigoMaterial = (int) Enumeradores.MaterialDeCarga.Soja,
                            CodigoFornecedor = _fornecedor3.Codigo,
                            Peso = 150
                        }

                });

            _quotasMock.Verify(x => x.Save(It.IsAny<Quota>()), Times.Exactly(2));
            _quotasMock.Verify(x => x.Delete(It.IsAny<Quota>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreErroAoSalvarQuotasFazRollback()
        {
            _quotasMock.Setup(x => x.FiltraPorData(It.IsAny<DateTime>()))
                       .Throws(new ExcecaoDeTeste("Ocorreu um erro ao consultar as quotas da data"));

            try
            {
                _cadastroQuota.Salvar(new List<QuotaSalvarVm>());
                Assert.Fail("Deveria ter gerado exceção");
            }
            catch (Exception)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ExcluirQuotaComAgendamentoException))]
        public void NaoPermiteExcluirUmaQuotaQueJaPossuiAgendamento()
        {
            var quotaSemAgendamento = DefaultObjects.ObtemQuotaDeCarregamento();
            var quotaComAgendamento = DefaultObjects.ObtemQuotaDeCarregamento();
            quotaComAgendamento.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
                {
                    Peso = 10,
                    Placa = "IQI3342"
                });

            _quotasMock.Setup(x => x.List()).Returns(new List<Quota>()
                {
                    quotaComAgendamento,quotaSemAgendamento
                });

            _quotasMock.Setup(x => x.Delete(It.IsAny<Quota>()));

            _cadastroQuota.Salvar(new List<QuotaSalvarVm>
                {
                        new QuotaSalvarVm
                        {
                            Data = DateTime.Today,
                            CodigoTerminal = "1000",
                            CodigoMaterial = (int) Enumeradores.MaterialDeCarga.Soja,
                            CodigoFornecedor = _fornecedor1.Codigo,
                            Peso = 100
                        }                    
                });

        }

        [TestMethod]
        public void NaoPermiteDiminuirAQuotaParaUmValorAbaixoDoQueJaEstaAgendado()
        {
            var quotaComAgendamento = DefaultObjects.ObtemQuotaDeCarregamento();
            quotaComAgendamento.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm
            {
                Peso = 100,
                Placa = "IQI3342"
            });

            _quotasMock.Setup(x => x.List()).Returns(new List<Quota>()
                {
                    quotaComAgendamento
                });

            _quotasMock.Setup(x => x.Delete(It.IsAny<Quota>()));

            _cadastroQuota.Salvar(new List<QuotaSalvarVm>
                {
                        new QuotaSalvarVm
                        {
                            Data = quotaComAgendamento.Data,
                            CodigoTerminal = quotaComAgendamento.CodigoTerminal,
                            CodigoMaterial = (int) Enumeradores.MaterialDeCarga.Soja,
                            CodigoFornecedor = quotaComAgendamento.Fornecedor.Codigo,
                            Peso = 90
                        }                    
                });
        }
    }
}
