using System;
using System.Linq;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaQuotaTests: RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            Usuario usuario = DefaultObjects.ObtemUsuarioPadrao();
            usuario.AdicionarPerfil(Enumeradores.Perfil.AgendadorDeCargas);
            DefaultPersistedObjects.PersistirUsuario(usuario);
            BaseTestClass.SubstituirUsuarioConectado(new UsuarioConectado(usuario.Login, usuario.Nome,usuario.Perfis));
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            BaseTestClass.RestaurarUsuarioConectado();
            Cleanup();
        }
        
        [TestMethod]
        public void QuandoConsultaQuotaEmUmaDataQuePossuiQuotaRetornaVerdadeiro()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000",DateTime.Today,1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsTrue(consultaQuota.PossuiQuotaNaData(DateTime.Today));
           
        }
        [TestMethod]
        public void QuandoConsultaQuotaEmUmaDataQueNaoPossuiQuotaRetornaFalso()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000", DateTime.Today, 1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsFalse(consultaQuota.PossuiQuotaNaData(DateTime.Today.AddDays(-10)));
            
        }

        [TestMethod]
        public void QuandoConsultaQuotasEmUmaDeterminadaDataRetornaListaDeQuotasDeTodosOsFornecedoresDaquelaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            IList<QuotaConsultarVm> quotas = consultaQuota.QuotasDaData(quota.Data);

            Assert.AreEqual(1, quotas.Count);
        }

        [TestMethod]
        public void QuandoConsultaQuotasDeUmDeterminadoFornecedorRetornaListaDeQuotasEmOrdemDecrescenteDeData()
        {
            //cria duas quotas para o mesmo fornecedor
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var quota1 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor, "1000",DateTime.Today,100);
            var quota2 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor, "1000", DateTime.Today.AddDays(1), 100);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            KendoGridVm kendoGridVm =
                consultaQuota.ListarQuotasDoFornecedor(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10},fornecedor.Codigo);

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            IList<QuotaPorFornecedorVm> quotas = kendoGridVm.Registros.Cast<QuotaPorFornecedorVm>().ToList();
            Assert.IsTrue(Convert.ToDateTime(quotas[0].Data) > Convert.ToDateTime(quotas[1].Data));
        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPeloNumeroDaNotaFiscal()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            //AgendamentoDeDescarregamento agendamento = DefaultObjects.ObtemAgendamentoDeDescarregamento(quota);
            var agendamentoVm = new AgendamentoDeDescarregamentoSalvarVm
                {
                    IdQuota = quota.Id,
                    Placa = "IOQ5338",
                    IdAgendamento = 0,
                    NotasFiscais = new List<NotaFiscalVm>
                        {
                            new NotaFiscalVm
                                {
                                    Numero = "1234",
                                    Serie = "1",
                                    DataDeEmissao = DateTime.Today.ToShortDateString(),
                                    CnpjDoEmitente = "123",
                                    NomeDoEmitente = "Emitente",
                                    CnpjDoContratante = "666",
                                    NomeDoContratante = "contratante",
                                    NumeroDoContrato = "4001",
                                    Peso = 100,
                                    Valor = 150
                                }
                        }

                };
            quota.InformarAgendamento(agendamentoVm);

            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            UnitOfWorkNh.Session.Clear();

            var filtro = new ConferenciaDeCargaFiltroVm
                {
                    CodigoTerminal = "1000",
                    NumeroNf = "1234"/*,
                    CodigoRealizacaoDeAgendamento = (int) Enumeradores.RealizacaoDeAgendamento.NaoRealizado*/
                };
            KendoGridVm kendoGridVm = consultaQuota.Consultar(new PaginacaoVm{Page = 1, PageSize = 10, Take = 10},  filtro);
            Assert.AreEqual(1,kendoGridVm.QuantidadeDeRegistros);
        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPelaPlaca()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            //AgendamentoDeDescarregamento agendamento = DefaultObjects.ObtemAgendamentoDeDescarregamento(quota);
            var agendamentoVm = new AgendamentoDeDescarregamentoSalvarVm
            {
                IdQuota = quota.Id,
                Placa = "IMN1620",
                IdAgendamento = 0,
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            new NotaFiscalVm
                                {
                                    Numero = "12345",
                                    Serie = "1",
                                    DataDeEmissao = DateTime.Today.ToShortDateString(),
                                    CnpjDoEmitente = "123",
                                    NomeDoEmitente = "Emitente",
                                    CnpjDoContratante = "666",
                                    NomeDoContratante = "contratante",
                                    NumeroDoContrato = "4001",
                                    Peso = 100,
                                    Valor = 150
                                }
                        }

            };
            quota.InformarAgendamento(agendamentoVm);

            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();

            UnitOfWorkNh.Session.Clear();

            var filtro = new ConferenciaDeCargaFiltroVm
            {
                CodigoTerminal = "1000",
                Placa = "IMN1620"
            };
            KendoGridVm kendoGridVm = consultaQuota.Consultar(new PaginacaoVm { Page = 1, PageSize = 10, Take = 10 }, filtro);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            
        }

    }
}
