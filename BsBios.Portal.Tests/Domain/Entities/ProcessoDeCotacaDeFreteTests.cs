using System;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class ProcessoDeCotacaoDeFreteTests
    {
        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoDeFreteAsPropriedadesFicamCorretas()
        {
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var dataValidadeInicial = DateTime.Today.AddMonths(1);
            var dataValidadeFinal = DateTime.Today.AddMonths(2);
            var processo = new ProcessoDeCotacaoDeFrete("Requisitos do Processo de Cotação de Fretes",
                "10001",dataLimiteDeRetorno , dataValidadeInicial, dataValidadeFinal, itinerario);

            processo.AdicionarItem(produto, 100, unidadeDeMedida);

            var item = processo.Itens.First();
            //Assert.AreSame(produto, processo.Produto);
            //Assert.AreEqual(100, processo.Quantidade);
            //Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreSame(produto, item.Produto);
            Assert.AreEqual(100, item.Quantidade);
            Assert.AreSame(unidadeDeMedida, item.UnidadeDeMedida);

            Assert.AreEqual("Requisitos do Processo de Cotação de Fretes",processo.Requisitos);
            Assert.AreEqual("10001", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);
        }

        [TestMethod]
        public void QuandoAtualizoUmaCotacaoDeFreteAsPropriedadesSaoAlteradas()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();

            var dataLimiteDeRetorno = DateTime.Today.AddDays(15);
            var dataValidadeInicial = DateTime.Today.AddMonths(2);
            var dataValidadeFinal = DateTime.Today.AddMonths(3);


            processo.Atualizar("requisitos alterados","1500",dataLimiteDeRetorno, 
                dataValidadeInicial, dataValidadeFinal, itinerario);

            processo.AtualizarItem(produto, 1500, unidadeDeMedida);

            var item = processo.Itens.First();
            //Assert.AreSame(produto, processo.Produto);
            //Assert.AreEqual(1500, processo.Quantidade);
            //Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreSame(produto, item.Produto);
            Assert.AreEqual(1500, item.Quantidade);
            Assert.AreSame(unidadeDeMedida, item.UnidadeDeMedida);
            Assert.AreEqual("requisitos alterados", processo.Requisitos);
            Assert.AreEqual("1500", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            processoDeCotacaoDeFrete.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeFrete.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            processoDeCotacaoDeFrete.Atualizar("requisitos alterados", "1500", processoDeCotacaoDeFrete.DataLimiteDeRetorno.Value,
                processoDeCotacaoDeFrete.DataDeValidadeInicial, processoDeCotacaoDeFrete.DataDeValidadeFinal, processoDeCotacaoDeFrete.Itinerario);
        }

        [TestMethod]
        [ExpectedException(typeof (AbrirProcessoDeCotacaoAbertoException))]
        public void QuandoTentarAbrirUmProcessoDeCotacaoQueJaEstaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComFornecedor();
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(FecharProcessoDeCotacaoFechadoException))]
        public void QuandoTentarFecharUmProcessoDeCotacaoQueJaEstaFechadoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado();
            processoDeCotacao.Fechar("texto de cabeçalho", "nota de cabeçalho");   
        }
    }
}
