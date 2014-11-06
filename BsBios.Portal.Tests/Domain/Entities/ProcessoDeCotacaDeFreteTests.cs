using System;
using System.Collections.Generic;
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
            var deposito = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("1000", "Torres");
            var municipioDestino = new Municipio("2000", "Porto Alegre");
            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();
            var processo = new ProcessoDeCotacaoDeFrete(produto, 100,unidadeDeMedida, "Requisitos do Processo de Cotação de Fretes",
                "10001",dataLimiteDeRetorno , dataValidadeInicial, dataValidadeFinal, itinerario,fornecedor, 1000,true,municipioOrigem, 
                municipioDestino, deposito,terminal,150);

            Assert.AreSame(produto, processo.Produto);
            Assert.AreEqual(100, processo.Quantidade);
            Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreEqual("Requisitos do Processo de Cotação de Fretes",processo.Requisitos);
            Assert.AreEqual("10001", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);
            Assert.AreSame(fornecedor, processo.FornecedorDaMercadoria);
            Assert.AreSame(municipioOrigem, processo.MunicipioDeOrigem);
            Assert.AreSame(municipioDestino, processo.MunicipioDeDestino);
            Assert.AreEqual(1000, processo.Cadencia);
            Assert.IsTrue(processo.Classificacao);
            Assert.AreSame(deposito, processo.Deposito);
            Assert.AreEqual(terminal, processo.Terminal);
            Assert.AreEqual(150, processo.ValorPrevisto);
        }

        [TestMethod]
        public void QuandoAtualizoUmaCotacaoDeFreteAsPropriedadesSaoAlteradas()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            var terminal2 = new Terminal("2000", "Terminal 2", "Passo Fundo");

            var dataLimiteDeRetorno = DateTime.Today.AddDays(15);
            var dataValidadeInicial = DateTime.Today.AddMonths(2);
            var dataValidadeFinal = DateTime.Today.AddMonths(3);
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("2000", "Porto Alegre");
            var municipioDestino = new Municipio("1500", "Torres");
            var deposito = DefaultObjects.ObtemFornecedorPadrao();

            processo.Atualizar(produto, 1500, unidadeDeMedida,"requisitos alterados","1500",dataLimiteDeRetorno,
                dataValidadeInicial, dataValidadeFinal, itinerario, fornecedor, 2000, false, municipioOrigem, 
                municipioDestino, deposito,terminal2,100);

            Assert.AreSame(produto, processo.Produto);
            Assert.AreEqual(1500, processo.Quantidade);
            Assert.AreSame(unidadeDeMedida, processo.UnidadeDeMedida);
            Assert.AreEqual("requisitos alterados", processo.Requisitos);
            Assert.AreEqual("1500", processo.NumeroDoContrato);
            Assert.AreEqual(dataLimiteDeRetorno, processo.DataLimiteDeRetorno);
            Assert.AreEqual(dataValidadeInicial, processo.DataDeValidadeInicial);
            Assert.AreEqual(dataValidadeFinal, processo.DataDeValidadeFinal);
            Assert.AreSame(itinerario, processo.Itinerario);
            Assert.AreSame(fornecedor, processo.FornecedorDaMercadoria);
            Assert.AreSame(municipioOrigem, processo.MunicipioDeOrigem);
            Assert.AreSame(municipioDestino, processo.MunicipioDeDestino);
            Assert.AreEqual(2000, processo.Cadencia);
            Assert.IsFalse(processo.Classificacao);
            Assert.AreSame(deposito, processo.Deposito);
            Assert.AreEqual(terminal2, processo.Terminal);

        }

        [TestMethod]
        [ExpectedException(typeof(DataDeValidadeInicialMaiorQueDataDeValidadeFinalException))]
        public void QuandoCrioUmProcessoComADataDeValidadeFinalMaiorQueADataInicialEsperoUmaExcecao()
        {

            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            var dataLimiteDeRetorno = DateTime.Today.AddDays(10);
            var dataValidadeInicial = DateTime.Today.AddMonths(1);
            var dataValidadeFinal = DateTime.Today.AddMonths(-1);
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var deposito = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("1000", "Torres");
            var municipioDestino = new Municipio("2000", "Porto Alegre");
            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();
            var processo = new ProcessoDeCotacaoDeFrete(produto, 100, unidadeDeMedida, "Requisitos do Processo de Cotação de Fretes",
                "10001", dataLimiteDeRetorno, dataValidadeInicial, dataValidadeFinal, itinerario, fornecedor, 1000, true, municipioOrigem,
                municipioDestino, deposito, terminal, 100);            
        }

        [TestMethod]
        [ExpectedException(typeof(DataDeValidadeInicialMaiorQueDataDeValidadeFinalException))]
        public void QuandoAtualizoUmProcessoComADataDeValidadeFinalMaiorQueADataInicialEsperoUmaExcecao()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            Itinerario itinerario = DefaultObjects.ObtemItinerarioPadrao();
            var terminal2 = new Terminal("2000", "Terminal 2", "Passo Fundo");

            var dataLimiteDeRetorno = DateTime.Today.AddDays(15);
            var dataValidadeInicial = DateTime.Today.AddMonths(2);
            var dataValidadeFinal = DateTime.Today.AddMonths(-2);
            var fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var municipioOrigem = new Municipio("2000", "Porto Alegre");
            var municipioDestino = new Municipio("1500", "Torres");
            var deposito = DefaultObjects.ObtemFornecedorPadrao();

            processo.Atualizar(produto, 1500, unidadeDeMedida, "requisitos alterados", "1500", dataLimiteDeRetorno,
                dataValidadeInicial, dataValidadeFinal, itinerario, fornecedor, 2000, false, municipioOrigem,
                municipioDestino, deposito, terminal2,200);


        }


        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAbertoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            processoDeCotacaoDeFrete.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeFrete.Abrir();

            Terminal terminal = DefaultObjects.ObtemTerminalPadrao();
            processoDeCotacaoDeFrete.Atualizar(processoDeCotacaoDeFrete.Produto, 1500, processoDeCotacaoDeFrete.UnidadeDeMedida, 
                "requisitos alterados", "1500", processoDeCotacaoDeFrete.DataLimiteDeRetorno.Value,
                processoDeCotacaoDeFrete.DataDeValidadeInicial, processoDeCotacaoDeFrete.DataDeValidadeFinal, processoDeCotacaoDeFrete.Itinerario,
                processoDeCotacaoDeFrete.FornecedorDaMercadoria,processoDeCotacaoDeFrete.Cadencia, processoDeCotacaoDeFrete.Classificacao,
                processoDeCotacaoDeFrete.MunicipioDeOrigem, processoDeCotacaoDeFrete.MunicipioDeDestino, processoDeCotacaoDeFrete.Deposito,terminal,100);
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
            IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = processoDeCotacao.FornecedoresSelecionados.Select(x => new CondicaoDoFechamentoNoSap
            {
                CodigoDoFornecedor = x.Fornecedor.Codigo,
                NumeroGeradoNoSap = "00001"
            });

            processoDeCotacao.FecharProcesso(condicoesDeFechamento);   
        }

        [TestMethod]
        [ExpectedException(typeof (FornecedorSemCondicaoGeradaNoSapException))]
        public void QuandoFecharUmProcessoDeCotacaoSemInformarONumeroDaCondicaoGeradaNoSapDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();

            processoDeCotacaoDeFrete.FecharProcesso(new List<CondicaoDoFechamentoNoSap>());
        }

        [TestMethod]
        public void QuandoTentarFecharUmProcessoDeCotacaoDeveAtribuirONumeroDaCondicaoGeradaNoSapParaCadaFornecedor()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComDuasCotacoesSelecionadas();
            var condicoesDeFechamento = new List<CondicaoDoFechamentoNoSap>
            {
                new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor  = processoDeCotacao.FornecedoresSelecionados.First().Fornecedor.Codigo ,
                    NumeroGeradoNoSap =  "0001"
                },
                new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor  = processoDeCotacao.FornecedoresSelecionados.Last().Fornecedor.Codigo ,
                    NumeroGeradoNoSap =  "0002"
                }


            };

            processoDeCotacao.FecharProcesso(condicoesDeFechamento);

            Assert.AreEqual("0001", ((CotacaoDeFrete) processoDeCotacao.FornecedoresSelecionados.First().Cotacao).NumeroDaCondicaoGeradaNoSap);
            Assert.AreEqual("0002", ((CotacaoDeFrete)processoDeCotacao.FornecedoresSelecionados.Last().Cotacao).NumeroDaCondicaoGeradaNoSap);

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

        [TestMethod]
        public void NaoPossoSelecionarUmFornecedorQueRecusoOProcessoDeCotacao()
        {

            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada();
            string codigoDoFornecedor = processoDeCotacaoDeFrete.FornecedoresParticipantes.Single().Fornecedor.Codigo;
            processoDeCotacaoDeFrete.DesativarParticipante(codigoDoFornecedor);

            try
            {
                processoDeCotacaoDeFrete.SelecionarCotacao(0,10,1);
                Assert.Fail("Deveria ter gerado exceção.");
            }
            catch (Exception exception)
            {
                Assert.AreEqual("Não é possível selecionar um fornecedor que recusou o processo de cotação.", exception.Message);
            }

            Assert.IsFalse(processoDeCotacaoDeFrete.FornecedoresParticipantes.Single().Cotacao.Selecionada);


        }
    }
}
