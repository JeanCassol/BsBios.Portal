using System;
using System.Configuration;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
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
        public void NaoEPermitidoAlterarAQuantidadeLiberadaParaUmaQuantidadeMenorQueAQuantidadeJaTransportada()
        {
            throw new NotImplementedException();
            
        }


    }
}
