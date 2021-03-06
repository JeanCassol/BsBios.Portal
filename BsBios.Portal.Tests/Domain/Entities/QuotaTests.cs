﻿using System;
using System.Linq;
using System.Collections.Generic;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class QuotaTests
    {
        [TestMethod]
        public void QuandoCrioUmaQuotaAsPropriedadesFicamCorretas()
        {
            Fornecedor transportadora = DefaultObjects.ObtemTransportadoraPadrao();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, transportadora, "1000", DateTime.Today, 1000);

            Assert.AreSame(transportadora, quota.Fornecedor);
            Assert.AreEqual("1000", quota.CodigoTerminal);
            Assert.AreEqual(DateTime.Today, quota.Data);
            Assert.AreEqual(Enumeradores.MaterialDeCarga.Soja,quota.Material);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quota.FluxoDeCarga);
            Assert.AreEqual(1000, quota.PesoTotal);
            Assert.AreEqual(0, quota.PesoAgendado);
            Assert.AreEqual(0, quota.PesoRealizado);
            Assert.AreEqual(1000, quota.PesoDisponivel);
        }

        [TestMethod]
        public void QuandoCriuoUmaQuotaDeSojaOFluxoEDescarregamento()
        {
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemFornecedorPadrao(), "1000", DateTime.Today, 1000);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Descarregamento, quota.FluxoDeCarga);
        }

        [TestMethod]
        public void QuandoCriuoUmaQuotaDeFareloOFluxoECarregamento()
        {
            var quota = new Quota(Enumeradores.MaterialDeCarga.Farelo, DefaultObjects.ObtemFornecedorPadrao(), "1000", DateTime.Today, 1000);
            Assert.AreEqual(Enumeradores.FluxoDeCarga.Carregamento, quota.FluxoDeCarga);
        }

        [TestMethod]
        public void QuandoAdicionoAgendamentosCalculaOPesoAgendadoCorretamente()
        {
            //peso total é 850
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
                {
                    IdQuota = quota.Id,
                    IdAgendamento = 0,
                    Placa = "IMN2420",
                    NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(180),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(230)
                        }
                });
            Assert.AreEqual(410, quota.PesoAgendado);
        }

        [TestMethod]
        [ExpectedException(typeof(PesoAgendadoSuperiorAoPesoDaQuotaException))]
        public void QuandoPesoAgendadoSuperiorPesoDaQuotaDeveDispararExcecao()
        {
            //peso total é 850
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(450),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(401)
                        }
            });
        }

        [TestMethod]
        [ExpectedException(typeof(AgendamentosSimultaneosParaMesmaPlacaException))]
        public void QuandoInformarMaisDeUmAgendamentoDeDescarregamentoNaoRealizadoParaMesmaPlacaDeveGerarExcecao()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(50),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(51)
                        }
            });

            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMn2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(52),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(53)
                        }
            });
        }

        [TestMethod]
        public void PermiteAdicionarUmSegundoAgendamentoDeDescarregamentoParaMesmaPlacaSeOPrimeiroEstiverRealizado()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeDescarregamento();
            AgendamentoDeDescarregamento agendamentoDeDescarregamento = quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(50),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(51)
                        }
            });

            agendamentoDeDescarregamento.Realizar();

            quota.InformarAgendamento(new AgendamentoDeDescarregamentoSalvarVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                NotasFiscais = new List<NotaFiscalVm>
                        {
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(52),
                            DefaultObjects.ObtemNotaFiscalVmComPesoEspecifico(53)
                        }
            });

            Assert.AreEqual(2, quota.Agendamentos.Count);
            Assert.AreEqual(2, quota.Agendamentos.Count(x => x.Placa == "IMN2420"));
            
        }

        [TestMethod]
        [ExpectedException(typeof(AgendamentosSimultaneosParaMesmaPlacaException))]
        public void QuandoInformarMaisDeUmAgendamentoDeCarregamentoNaoRealizadoParaMesmaPlacaDeveGerarExcecao()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                Peso = 10
            });

            quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMn2420",
                Peso = 20
            });
        }

        [TestMethod]
        public void PermiteAdicionarUmSegundoAgendamentoDeCarregamentoParaMesmaPlacaSeOPrimeiroEstiverRealizado()
        {
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            AgendamentoDeCarregamento agendamentoDeDescarregamento = quota
                .InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                Peso = 10
            });

            agendamentoDeDescarregamento.Realizar();

            quota.InformarAgendamento(new AgendamentoDeCarregamentoCadastroVm()
            {
                IdQuota = quota.Id,
                IdAgendamento = 0,
                Placa = "IMN2420",
                Peso = 20
            });

            Assert.AreEqual(2, quota.Agendamentos.Count);
            Assert.AreEqual(2, quota.Agendamentos.Count(x => x.Placa == "IMN2420"));

        }

    }
}
