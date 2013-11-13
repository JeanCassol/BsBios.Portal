using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
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

            var ordemDeTransporte = processoDeCotacao.FecharProcesso().First();

            Assert.AreSame(processoDeCotacao, ordemDeTransporte.ProcessoDeCotacaoDeFrete);
            Assert.AreSame(fornecedorParticipante.Fornecedor, ordemDeTransporte.Fornecedor);
            Assert.AreEqual(quantidade, ordemDeTransporte.QuantidadeLiberada);
            Assert.AreEqual(quantidade, ordemDeTransporte.QuantidadeAdquirida);

        }

        [TestMethod]
        public void PossoAlterarAQuantidadeLiberadaParaUmValorValido()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            ordemDeTransporte.AlterarQuantidadeLiberada(8M);
            Assert.AreEqual(8M, ordemDeTransporte.QuantidadeLiberada);
        }

        [TestMethod]
        [ExpectedException(typeof(QuantidadeLiberadaSuperouQuantidadeAdquiridaException))]
        public void NaoEPermitidoAlterarAQuantidadeLiberadaParaUmaQuantidadeMaiorQueAQuantidadeAdquiridaNoProcessoDeCotacao()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            ordemDeTransporte.AlterarQuantidadeLiberada(11M);

        }

        [TestMethod]
        [ExpectedException(typeof(QuantidadeLiberadaAbaixoDaQuantidadeColetadaException))]
        public void NaoEPermitidoAlterarAQuantidadeLiberadaParaUmaQuantidadeMenorQueAQuantidadeJaColetada()
        {
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(9M);
            var fabricaDeColeta = new ColetaFactory();
            var coletaSalvarVm = new ColetaSalvarVm
            {
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

            ordemDeTransporte.AlterarQuantidadeLiberada(5M);
            
        }


    }
}
