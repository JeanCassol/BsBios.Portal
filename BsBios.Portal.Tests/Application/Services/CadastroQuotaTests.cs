using System;
using System.Collections.Generic;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
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
        private readonly Mock<ITerminais> _terminaisMock;
        private readonly Mock<IMateriaisDeCarga> _materiasDeCargaMock;
        private readonly ICadastroQuota _cadastroQuota;
        private readonly Fornecedor _fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
        private readonly Fornecedor _fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
        private readonly Fornecedor _fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
        private readonly Terminal _terminal = DefaultObjects.ObtemTerminalPadrao();

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

            _quotasMock.Setup(x => x.DoTerminal(It.IsAny<string>()))
                .Returns(_quotasMock.Object);

            _quotasMock.Setup(x => x.List()).Returns(new List<Quota>()
                {
                    new Quota(DefaultObjects.ObterSoja(), Enumeradores.FluxoDeCarga.Descarregamento, _fornecedor1,_terminal,DateTime.Today,100 ),
                    new Quota(DefaultObjects.ObterSoja(), Enumeradores.FluxoDeCarga.Descarregamento, _fornecedor2,_terminal,DateTime.Today,120 )
                });

            _fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            _fornecedoresMock.Setup(x => x.BuscaListaPorCodigo(It.IsAny<string[]>()))
                             .Returns(_fornecedoresMock.Object);
            _fornecedoresMock.Setup(x => x.List()).Returns(new List<Fornecedor>
                {
                    _fornecedor3
                });

            _terminaisMock = new Mock<ITerminais>(MockBehavior.Strict);
            _terminaisMock.Setup(x => x.BuscaPeloCodigo(It.IsAny<string>()))
                .Returns(_terminal);

            _materiasDeCargaMock = new Mock<IMateriaisDeCarga>(MockBehavior.Strict);

            _materiasDeCargaMock.Setup(x => x.BuscarLista(It.IsAny<int[]>())).Returns(_materiasDeCargaMock.Object);
            _materiasDeCargaMock.Setup(x => x.List()).Returns(new List<MaterialDeCarga> {DefaultObjects.ObterSoja()});

            _cadastroQuota = new CadastroQuota(_unitOfWorkMock.Object, _quotasMock.Object, _fornecedoresMock.Object,_terminaisMock.Object, _materiasDeCargaMock.Object);
        }

        [TestMethod]
        public void QuandoCadastroQuotasComSucessoOcorrePersistencia()
        {
            var quotasSalvarVm = new QuotasSalvarVm
            {
                Data = DateTime.Today,
                CodigoDoTerminal = "1000",
                Quotas = new List<QuotaSalvarVm>
                {
                    new QuotaSalvarVm
                    {
                        CodigoMaterial = 0,
                        CodigoFornecedor = _fornecedor1.Codigo,
                        FluxoDeCarga = 2,
                        Peso = 100
                    },
                    new QuotaSalvarVm
                    {
                        CodigoMaterial = 0,
                        CodigoFornecedor = _fornecedor3.Codigo,
                        FluxoDeCarga = 2,
                        Peso = 150
                    }
                }
            };
            //caso de uso: para a data de hoje tenho duas quotas cadastradas:uma para o fornecedor1 e outra para o fornecedor3
            //salvo as cotações informando os fornecedores 1 e 3. O resultado esperado é que a quota do fornecedor1 seja atualizada,
            //a quota do fornecedor2 seja atualizada e a quota do fornecedor seja criada.
            _cadastroQuota.Salvar(quotasSalvarVm);

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
                _cadastroQuota.Salvar(new QuotasSalvarVm());
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

            var quotasSalvarVm = new QuotasSalvarVm
            {
                Data = DateTime.Today,
                CodigoDoTerminal = "1000",
                Quotas = new List<QuotaSalvarVm>
                {
                    new QuotaSalvarVm
                    {
                        
                        CodigoMaterial = 1,
                        CodigoFornecedor = _fornecedor1.Codigo,
                        Peso = 100
                    }
                }
            };

            _cadastroQuota.Salvar(quotasSalvarVm);

        }

        [TestMethod]
        [ExpectedException(typeof(PesoAgendadoSuperiorAoPesoDaQuotaException))]
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

            var quotasSalvarVm = new QuotasSalvarVm
            {
                Data = DateTime.Today,
                CodigoDoTerminal = quotaComAgendamento.Terminal.Codigo,
                Quotas = new List<QuotaSalvarVm>
                {
                    new QuotaSalvarVm
                    {
                        CodigoMaterial =  quotaComAgendamento.Material.Codigo,
                        CodigoFornecedor = quotaComAgendamento.Fornecedor.Codigo,
                        FluxoDeCarga = (int) quotaComAgendamento.FluxoDeCarga,
                        Peso = 90
                    }
                }
            };
            _cadastroQuota.Salvar(quotasSalvarVm);
        }

        [TestMethod]
        public void ConsigoRemoverTodasAsQuotasDeUmaData()
        {
            _quotasMock.Setup(x => x.Delete(It.IsAny<Quota>()));
            var quotasSalvarVm = new QuotasSalvarVm
            {
                CodigoDoTerminal = "1000",
                Data = DateTime.Today,
                Quotas = new List<QuotaSalvarVm>()
            };
            _cadastroQuota.Salvar(quotasSalvarVm);
            _quotasMock.Verify(x => x.Delete(It.IsAny<Quota>()), Times.Exactly(2));
            _quotasMock.Verify(x => x.Save(It.IsAny<Quota>()), Times.Never());
        }
    }
}
