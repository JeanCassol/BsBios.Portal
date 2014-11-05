using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class OrdemDeTransporteTests
    {
        [TestMethod]
        public void QuandoCrioUmaOrdemDeTransporteAPartirDeUmProcessoDeCotacaoAsPropriedadasFicamCorretas()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresSelecionados.First();
            Cotacao cotacao = fornecedorParticipante.Cotacao;
            decimal quantidade = cotacao.QuantidadeAdquirida.Value;

            var condicoesDeFechamento = new List<CondicaoDoFechamentoNoSap>
            {
                new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                    NumeroGeradoNoSap = "00001"
                }
            };

            var ordemDeTransporte = processoDeCotacao.FecharProcesso(condicoesDeFechamento).First();

            Assert.AreSame(processoDeCotacao, ordemDeTransporte.ProcessoDeCotacaoDeFrete);
            Assert.AreSame(fornecedorParticipante.Fornecedor, ordemDeTransporte.Fornecedor);
            Assert.AreEqual(quantidade, ordemDeTransporte.QuantidadeLiberada);
            Assert.AreEqual(quantidade, ordemDeTransporte.QuantidadeAdquirida);

        }

        [TestMethod]
        public void PossoAlterarAQuantidadeLiberadaParaUmValorValido()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            ordemDeTransporte.AlterarQuantidades(8M,1M);
            Assert.AreEqual(8M, ordemDeTransporte.QuantidadeLiberada);
            Assert.AreEqual(1M, ordemDeTransporte.QuantidadeDeTolerancia);
        }

        [TestMethod]
        [ExpectedException(typeof(QuantidadeLiberadaAbaixoDaQuantidadeColetadaException))]
        public void NaoEPermitidoAlterarAQuantidadeLiberadaParaUmaQuantidadeMenorQueAQuantidadeJaColetada()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            var fabricaDeColeta = new ColetaFactory();
            var coletaSalvarVm = new ColetaSalvarVm
            {
                DataDaColeta = DateTime.Now.Date.AddDays(-1).ToShortDateString(),
                DataDePrevisaoDeChegada = DateTime.Now.Date.ToShortDateString(),
                IdDaOrdemDeTransporte = ordemDeTransporte.Id,
                Motorista = "Mauro",
                Peso = 8,
                Placa = "ioq-5338",
                Realizado = "Não",
                NotasFiscais = new List<NotaFiscalDeColetaVm>
                {
                    new NotaFiscalDeColetaVm
                    {
                        DataDeEmissao = DateTime.Now.Date.ToShortDateString(),
                        Peso = 8,
                        Numero = "123434",
                        Serie = "1",
                        Valor = 5435M
                    }
                }

            };

            Coleta coleta = fabricaDeColeta.Construir(coletaSalvarVm, ordemDeTransporte.PrecoUnitario);
            ordemDeTransporte.AdicionarColeta(coleta);

            ordemDeTransporte.AlterarQuantidades(5M, 0M);
            
        }

        [TestMethod]
        public void UmaOrdemDeTransporteIniciaComOStatusAberto()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            Assert.AreEqual(Enumeradores.StatusParaColeta.Aberto, ordemDeTransporte.StatusParaColeta);
        }

        [TestMethod]
        [ExpectedException(typeof(AlterarOrdemDeTransporteFechadaException))]
        public void NaoEPermitidoAdicionarColetaEmUmaOrdemDeTransporteFechada()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            var fabricaDeColeta = new ColetaFactory();
            var coletaSalvarVm = new ColetaSalvarVm
            {
                DataDaColeta = DateTime.Now.Date.AddDays(-1).ToShortDateString(),
                DataDePrevisaoDeChegada = DateTime.Now.Date.ToShortDateString(),
                IdDaOrdemDeTransporte = ordemDeTransporte.Id,
                Motorista = "Mauro",
                Peso = 8,
                Placa = "ioq-5338",
                Realizado = "Não",
                NotasFiscais = new List<NotaFiscalDeColetaVm>
                {
                    new NotaFiscalDeColetaVm
                    {
                        DataDeEmissao = DateTime.Now.Date.ToShortDateString(),
                        Peso = 8,
                        Numero = "123434",
                        Serie = "1",
                        Valor = 5435M
                    }
                }

            };

            ordemDeTransporte.FecharParaColeta(Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte.AlteracaoDeLocalDeColeta,  "sem frota");

            Coleta coleta = fabricaDeColeta.Construir(coletaSalvarVm, ordemDeTransporte.PrecoUnitario);

            ordemDeTransporte.AdicionarColeta(coleta);            
        }

        [TestMethod]
        [ExpectedException(typeof(AlterarOrdemDeTransporteFechadaException))]
        public void NaoEPermitidoRemoverColetaDeUmaOrdemDeTransporteFechada()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComColeta();

            Coleta coleta = ordemDeTransporte.Coletas.First();

            ordemDeTransporte.FecharParaColeta(Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte.AlteracaoDeLocalDeColeta, "sem frota");

            ordemDeTransporte.RemoverColeta(coleta.Id);
        }

        [TestMethod]
        public void QuandoFechaUmaOrdemDeTransporteQueNaoFoiTotalmenteRealizadaQuantidadeLiberadaFicaIgualAQuantidadeColetada()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            var fabricaDeColeta = new ColetaFactory();
            var coletaSalvarVm = new ColetaSalvarVm
            {
                DataDaColeta = DateTime.Now.Date.AddDays(-1).ToShortDateString(),
                DataDePrevisaoDeChegada = DateTime.Now.Date.ToShortDateString(),
                IdDaOrdemDeTransporte = ordemDeTransporte.Id,
                Motorista = "Mauro",
                Peso = 8,
                Placa = "ioq-5338",
                Realizado = "Não",
                NotasFiscais = new List<NotaFiscalDeColetaVm>
                {
                    new NotaFiscalDeColetaVm
                    {
                        DataDeEmissao = DateTime.Now.Date.ToShortDateString(),
                        Peso = 8,
                        Numero = "123434",
                        Serie = "1",
                        Valor = 5435M
                    }
                }

            };

            ordemDeTransporte.AdicionarColeta(fabricaDeColeta.Construir(coletaSalvarVm, 10));

            const string motivo = "ponte estragada";

            ordemDeTransporte.FecharParaColeta(Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte.AlteracaoDeLocalDeColeta,  motivo);

            Assert.AreEqual(8, ordemDeTransporte.QuantidadeLiberada);

            Assert.AreEqual(motivo, ordemDeTransporte.ObservacaoDeFechamento);
            Assert.AreEqual(Enumeradores.MotivoDeFechamentoDaOrdemDeTransporte.AlteracaoDeLocalDeColeta, ordemDeTransporte.MotivoDeFechamento);

        }

    }
}
