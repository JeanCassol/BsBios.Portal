using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class CotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmaCotacaoAsPropriedadesSaoIniciadasCorretamente()
        {
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var fornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm,"Descrição do Incoterm", Enumeradores.TipoDeFrete.Cif);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm",cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(Enumeradores.TipoDeFrete.Cif, cotacao.TipoDeFrete);
        }

        [TestMethod]
        public void QuandoCrioUmItemDeCotacaoAsPropriedasSaoIniciadasCorretamente()
        {
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var fornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm, "Descrição do Incoterm", Enumeradores.TipoDeFrete.Cif);
            var itemDoProcesso = processoDeCotacao.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem) cotacao.InformarCotacaoDeItem(itemDoProcesso, 110,5, 80, DateTime.Today.AddMonths(1), "observacoes");

            Assert.IsFalse(cotacaoItem.Selecionada);
            Assert.IsNull(cotacaoItem.Iva);
            Assert.IsNull(cotacaoItem.QuantidadeAdquirida);
            Assert.AreEqual(110, cotacaoItem.ValorLiquido);
            Assert.AreEqual(110, cotacaoItem.ValorComImpostos);
            Assert.AreEqual(5, cotacaoItem.Mva);
            Assert.AreEqual(80, cotacaoItem.QuantidadeDisponivel);
            Assert.AreEqual(DateTime.Today.AddMonths(1), cotacaoItem.PrazoDeEntrega);
            Assert.AreEqual("observacoes", cotacaoItem.Observacoes);
        }

        [TestMethod]
        public void QuandoAtualizaoUmaCotacaoAsPropriedadesSaoCriadasCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var condicaoDePagamento = new CondicaoDePagamento("C1","CONDIÇÃO ALTERAÇÃO");
            var incoterm = new Incoterm("I1", "INCOTERM ALTERAÇÃO");
            var cotacao = (CotacaoMaterial) processoDeCotacao.FornecedoresParticipantes.First().Cotacao;
            cotacao.Atualizar(condicaoDePagamento, incoterm, "INCOTERM ALTERADO", Enumeradores.TipoDeFrete.Fob);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("INCOTERM ALTERADO", cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(Enumeradores.TipoDeFrete.Fob, cotacao.TipoDeFrete);
        }

        [TestMethod]
        public void QuandoAtualizoUmItemDeCotacaoAsPropriedadesSaoAlteradasCorretamente()
        {
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var fornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm, "Descrição do Incoterm", Enumeradores.TipoDeFrete.Cif);
            var itemDoProcesso = processoDeCotacao.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem)cotacao.InformarCotacaoDeItem(itemDoProcesso, 110, 0, 80, DateTime.Today.AddMonths(1), "observacoes");
            cotacaoItem.Atualizar(220, 10,90, DateTime.Today.AddMonths(2), "observacoes alteradas");


            Assert.AreEqual(220, cotacaoItem.ValorLiquido);
            Assert.AreEqual(220, cotacaoItem.ValorComImpostos);
            Assert.AreEqual(10, cotacaoItem.Mva);
            Assert.AreEqual(DateTime.Today.AddMonths(2), cotacaoItem.PrazoDeEntrega);
            Assert.AreEqual(90, cotacaoItem.QuantidadeDisponivel);
            Assert.AreEqual("observacoes alteradas", cotacaoItem.Observacoes);
        }

        [TestMethod]
        public void QuandoInformoUmImpostoParaACotacaoOImpostoEAdicionadoCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacaoItem = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();
            
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            Assert.AreEqual(1, cotacaoItem.Impostos.Count);
            Imposto imposto = cotacaoItem.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(17, imposto.Aliquota);
            Assert.AreEqual(34, imposto.Valor);
        }

        [TestMethod]
        public void QuandoUmImpostoJaExistenteeEInformadoNovamenteOMesmoEAtualizado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacaoItem = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12, 25);
            Assert.AreEqual(1, cotacaoItem.Impostos.Count);
            Imposto imposto = cotacaoItem.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(12, imposto.Aliquota);
            Assert.AreEqual(25, imposto.Valor);
        }

        //[TestMethod]
        //[ExpectedException(typeof(ValorTotalComImpostosObrigatorioException))]
        //public void QuandoAdicionoImpostoEmUmaCotacaoQueNaoPossuiValorTotalComImpostosDeveGerarExcecao()
        //{
        //    var cotacao = new Cotacao(DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
        //                              DefaultObjects.ObtemIncotermPadrao(), "INC", 100, null, null);

        //    cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 17);

        //}

        [TestMethod]
        [ExpectedException(typeof (MvaNaoInformadoException))]
        public void QuandoACotacaoPossuiIcmsDeSubstituicaoTributariaEoCampoMvaNaoForPreenchidoDeveDispararExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var codigoFornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            ProcessoDeCotacaoItem processoDeCotacaoItem = processoDeCotacao.Itens.First();
            var cotacao = processoDeCotacao.InformarCotacao(codigoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),DefaultObjects.ObtemIncotermPadrao(), "INC",Enumeradores.TipoDeFrete.Cif);
            var cotacaoItem = cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 100, null, 100, DateTime.Today, null);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17, 17);
        }

        [TestMethod]
        public void QuandoInformoUmaCotacaoComImpostosOvalorLiquidoECalculadoCorretamente()
        {

            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacaoItem = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();

            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12, 12);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 5, 5);
            //valor total com impostos da cotação padrão é 125. Soma dos valores dos impostos é 12+17+5 = 34
            //valor liquido = valor com impostos - valor dos impostos = 125 - 34 = 91
            Assert.AreEqual(91, cotacaoItem.ValorLiquido);

        }
        //[TestMethod]
        //public void ConsigoFazerCastEmUmaCotacaoDeMaterial()
        //{
        //    ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
        //    Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
        //    processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
        //                             DefaultObjects.ObtemIncotermPadrao(),
        //                             ",", 100, 110, 120, DateTime.Today, "");

        //    foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
        //    {
        //        var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao;
        //        Assert.IsNotNull(cotacao.CondicaoDePagamento);
        //    }

        //}
        //[TestMethod]
        //public void ConsigoFazerCastEmUmaCotacaoDeFrete()
        //{
        //    ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
        //    Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
        //    processo.AdicionarFornecedor(fornecedor);
        //    processo.Abrir();

        //    Cotacao cotacaoDeFrete =  processo.InformarCotacao(fornecedor.Codigo,1000,100,"");
        //    processo.SelecionarCotacao(cotacaoDeFrete.Id,150);

        //    foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
        //    {
        //        var cotacao = (CotacaoFrete)fornecedorParticipante.Cotacao;
        //        Assert.IsTrue(cotacao.Selecionada);
        //    }

        //}

        
    }
}
