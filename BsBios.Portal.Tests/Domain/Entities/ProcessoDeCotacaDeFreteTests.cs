using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
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
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("1000", "Torres");
            var municipioDestino = new Municipio("2000", "Porto Alegre");
            var processo = new ProcessoDeCotacaoDeFrete(produto, 100,unidadeDeMedida, "Requisitos do Processo de Cotação de Fretes",
                "10001",dataLimiteDeRetorno , dataValidadeInicial, dataValidadeFinal, itinerario,fornecedor, 1000,true,municipioOrigem, municipioDestino);

            Assert.AreSame(produto, processo.Produto);
            Assert.AreEqual(100, processo.Quantidade);
            Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreEqual("Requisitos do Processo de Cotação de Fretes",processo.Requisitos);
            Assert.AreEqual("10001", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);
            Assert.AreSame(fornecedor, processo.Fornecedor);
            Assert.AreSame(municipioOrigem, processo.MunicipioDeOrigem);
            Assert.AreSame(municipioDestino, processo.MunicipioDeDestino);
            Assert.AreEqual(1000, processo.Cadencia);
            Assert.IsTrue(processo.Classificacao);
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
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("2000", "Porto Alegre");
            var municipioDestino = new Municipio("1500", "Torres");


            processo.Atualizar(produto, 1500, unidadeDeMedida,"requisitos alterados","1500",dataLimiteDeRetorno,
                dataValidadeInicial, dataValidadeFinal, itinerario, fornecedor, 2000, false, municipioOrigem, municipioDestino);

            Assert.AreSame(produto, processo.Produto);
            Assert.AreEqual(1500, processo.Quantidade);
            Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreEqual("requisitos alterados", processo.Requisitos);
            Assert.AreEqual("1500", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);
            Assert.AreSame(fornecedor, processo.Fornecedor);
            Assert.AreSame(municipioOrigem, processo.MunicipioDeOrigem);
            Assert.AreSame(municipioDestino, processo.MunicipioDeDestino);
            Assert.AreEqual(2000, processo.Cadencia);
            Assert.IsFalse(processo.Classificacao);


        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAbertoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            processoDeCotacaoDeFrete.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeFrete.Abrir();
            processoDeCotacaoDeFrete.Atualizar(processoDeCotacaoDeFrete.Produto, 1500, processoDeCotacaoDeFrete.UnidadeDeMedida, 
                "requisitos alterados", "1500", processoDeCotacaoDeFrete.DataLimiteDeRetorno.Value,
                processoDeCotacaoDeFrete.DataDeValidadeInicial, processoDeCotacaoDeFrete.DataDeValidadeFinal, processoDeCotacaoDeFrete.Itinerario,
                processoDeCotacaoDeFrete.Fornecedor,processoDeCotacaoDeFrete.Cadencia, processoDeCotacaoDeFrete.Classificacao,
                processoDeCotacaoDeFrete.MunicipioDeOrigem, processoDeCotacaoDeFrete.MunicipioDeDestino);
        }

        [TestMethod]
        [ExpectedException(typeof (AbrirProcessoDeCotacaoAbertoException))]
        public void QuandoTentarAbrirUmProcessoDeCotacaoQueJaEstaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComFornecedor();
            processoDeCotacao.Abrir();
            processoDeCotacao.Abrir();
        }

        [TestMethod]
        [ExpectedException(typeof(FecharProcessoDeCotacaoFechadoException))]
        public void QuandoTentarFecharUmProcessoDeCotacaoQueJaEstaFechadoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado();
            processoDeCotacao.Fechar();   
        }

        [TestMethod]
        public void QuandoCanceloProcessoDeCotacaoDeFretePassaParaStatusCancelado()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            Assert.AreNotEqual(Enumeradores.StatusProcessoCotacao.Cancelado, processo.Status);
            processo.Cancelar();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Cancelado, processo.Status);

        }

        [TestMethod]
        [ExpectedException(typeof(CancelarProcessoDeCotacaoFechadoException))]
        public void NaoConsigoCancelarUmProcessoDeCotacaoFechado()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado();
            processo.Cancelar();
        }

    }
}
