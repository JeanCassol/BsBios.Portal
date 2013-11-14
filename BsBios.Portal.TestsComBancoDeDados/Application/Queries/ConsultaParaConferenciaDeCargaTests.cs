using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaParaConferenciaDeCargaTests : RepositoryTest
    {

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);

            RemoveQueries.RemoverQuotasCadastradas();

            Quota quota1 = DefaultObjects.ObtemQuotaDeDescarregamento();

            var agendamento1Vm = new AgendamentoDeDescarregamentoSalvarVm
            {
                IdQuota = quota1.Id,
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
            var agendamento2Vm = new AgendamentoDeDescarregamentoSalvarVm
            {
                IdQuota = quota1.Id,
                Placa = "IOQ5338",
                IdAgendamento = 0,
                NotasFiscais = new List<NotaFiscalVm>
                {
                    new NotaFiscalVm
                    {
                        Numero = "12346",
                        Serie = "1",
                        DataDeEmissao = DateTime.Today.AddDays(1).ToShortDateString(),
                        CnpjDoEmitente = "123",
                        NomeDoEmitente = "Mauro",
                        CnpjDoContratante = "666",
                        NomeDoContratante = "contratante",
                        NumeroDoContrato = "4001",
                        Peso = 100,
                        Valor = 150
                    }
                }

            };

            quota1.InformarAgendamento(agendamento1Vm);
            quota1.InformarAgendamento(agendamento2Vm);

            DefaultPersistedObjects.PersistirQuota(quota1);

            var primeiroAgendamento = quota1.Agendamentos.First();
            quota1.RealizarAgendamento(primeiroAgendamento.Id);

            DefaultPersistedObjects.PersistirQuota(quota1);

            Quota quota2 = DefaultObjects.ObtemQuotaDeCarregamentoComDataEspecifica(DateTime.Today);

            var agendamento3Vm = new AgendamentoDeDescarregamentoSalvarVm
            {
                IdQuota = quota1.Id,
                Placa = "IMN1620",
                IdAgendamento = 0,
                NotasFiscais = new List<NotaFiscalVm>
                {
                    new NotaFiscalVm
                    {
                        Numero = "552564",
                        Serie = "1",
                        DataDeEmissao = DateTime.Today.ToShortDateString(),
                        CnpjDoEmitente = "123",
                        NomeDoEmitente = "Fornecedor 3",
                        CnpjDoContratante = "666",
                        NomeDoContratante = "contratante",
                        NumeroDoContrato = "4001",
                        Peso = 100,
                        Valor = 150
                    }
                }

            };

            quota2.InformarAgendamento(agendamento3Vm);

            DefaultPersistedObjects.PersistirQuota(quota2);

            UnitOfWorkNh.Session.Clear();
        }

        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPeloNumeroDaNotaFiscal()
        {

            var consultaQuota = ObjectFactory.GetInstance<IConsultaParaConferenciaDeCargas>();

            var filtro = new ConferenciaDeCargaFiltroVm
            {
                CodigoTerminal = "1000",
                NumeroNf = "12345"
            };
            KendoGridVm kendoGridVm = consultaQuota.Consultar(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10},
                filtro);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var registro = kendoGridVm.Registros.Cast<ConferenciaDeCargaPesquisaResultadoVm>().First();
            Assert.AreEqual("12345", registro.NumeroNf);
        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPelaPlaca()
        {
            var consultaQuota = ObjectFactory.GetInstance<IConsultaParaConferenciaDeCargas>();

            var filtro = new ConferenciaDeCargaFiltroVm
            {
                CodigoTerminal = "1000",
                Placa = "IOQ5338"
            };
            KendoGridVm kendoGridVm = consultaQuota.Consultar(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10},
                filtro);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var registro = kendoGridVm.Registros.Cast<ConferenciaDeCargaPesquisaResultadoVm>().First();
            Assert.AreEqual(filtro.Placa, registro.Placa);

        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPeloFornecedor()
        {

            var filtro = new ConferenciaDeCargaFiltroVm
            {
                CodigoTerminal = "1000",
                NomeDoFornecedor = "Emit"
            };

            var consultaParaConferenciaDeCargas = ObjectFactory.GetInstance<IConsultaParaConferenciaDeCargas>();

            KendoGridVm kendoGridVm =
                consultaParaConferenciaDeCargas.Consultar(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10}, filtro);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var registro = kendoGridVm.Registros.Cast<ConferenciaDeCargaPesquisaResultadoVm>().First();
            Assert.AreEqual("Emitente", registro.NomeEmitente);

        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPelaData()
        {

            UnitOfWorkNh.Session.Clear();

            var filtro = new ConferenciaDeCargaFiltroVm{
                CodigoTerminal = "1000",
                DataAgendamento = DateTime.Today.ToShortDateString()
            };

            var consultaParaConferenciaDeCargas = ObjectFactory.GetInstance<IConsultaParaConferenciaDeCargas>();

            KendoGridVm kendoGridVm =
                consultaParaConferenciaDeCargas.Consultar(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10}, filtro);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var registro = kendoGridVm.Registros.Cast<ConferenciaDeCargaPesquisaResultadoVm>().First();
            Assert.AreEqual(DateTime.Today, registro.DataAgendamento);
            Assert.AreEqual(DateTime.Today.ToShortDateString(), registro.DataDeAgendamentoFormatada);

        }

        [TestMethod]
        public void ConsigoConsultarUmAgendamentoPelaInformacaoDeRealizado()
        {

            var filtro = new ConferenciaDeCargaFiltroVm
            {
                CodigoTerminal = "1000",
                RealizacaoDeAgendamento = Enumeradores.RealizacaoDeAgendamento.NaoRealizado
            };

            var consultaParaConferenciaDeCargas = ObjectFactory.GetInstance<IConsultaParaConferenciaDeCargas>();

            KendoGridVm kendoGridVm =
                consultaParaConferenciaDeCargas.Consultar(new PaginacaoVm { Page = 1, PageSize = 10, Take = 10 }, filtro);
            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var registro = kendoGridVm.Registros.Cast<ConferenciaDeCargaPesquisaResultadoVm>().First();
            Assert.IsFalse(registro.Realizado);

        }

    }
}
    