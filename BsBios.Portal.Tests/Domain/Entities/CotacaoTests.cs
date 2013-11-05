using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain;
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
            var cotacao = new CotacaoMaterial(condicaoDePagamento, incoterm, "Descrição do Incoterm", 110, 5, 80,DateTime.Today.AddMonths(1),"observacoes");
            Assert.IsFalse(cotacao.Selecionada);
            Assert.IsNull(cotacao.Iva);
            Assert.IsNull(cotacao.QuantidadeAdquirida);
            Assert.AreEqual(110, cotacao.ValorLiquido);
            Assert.AreEqual(110, cotacao.ValorComImpostos);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm",cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(5, cotacao.Mva);
            Assert.AreEqual(80, cotacao.QuantidadeDisponivel);
            Assert.AreEqual(DateTime.Today.AddMonths(1),cotacao.PrazoDeEntrega);
            Assert.AreEqual("observacoes", cotacao.Observacoes);
        }

        [TestMethod]
        public void QuandoAtualizaoUmaCotacaoAsPropriedadesSaoCriadasCorretamente()
        {
            CotacaoMaterial cotacao = DefaultObjects.ObtemCotacaoDeMaterialPadrao();
            var condicaoDePagamento = new CondicaoDePagamento("C1","CONDIÇÃO ALTERAÇÃO");
            var incoterm = new Incoterm("I1", "INCOTERM ALTERAÇÃO");
            cotacao.Atualizar(220, condicaoDePagamento, incoterm, "INCOTERM ALTERADO", 10, 90, DateTime.Today.AddMonths(2), "observacoes alteradas");
            Assert.AreEqual(220, cotacao.ValorLiquido);
            Assert.AreEqual(220, cotacao.ValorComImpostos);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("INCOTERM ALTERADO", cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(10, cotacao.Mva);
            Assert.AreEqual(DateTime.Today.AddMonths(2), cotacao.PrazoDeEntrega);
            Assert.AreEqual(90, cotacao.QuantidadeDisponivel);
            Assert.AreEqual("observacoes alteradas", cotacao.Observacoes);

        }

        [TestMethod]
        public void QuandoInformoUmImpostoParaACotacaoOImpostoEAdicionadoCorretamente()
        {
            Cotacao cotacao = DefaultObjects.ObtemCotacaoDeMaterialPadrao();
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            Assert.AreEqual(1, cotacao.Impostos.Count);
            Imposto imposto = cotacao.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(17, imposto.Aliquota);
            Assert.AreEqual(34, imposto.Valor);
        }

        [TestMethod]
        public void QuandoUmImpostoJaExistenteeEInformadoNovamenteOMesmoEAtualizado()
        {
            Cotacao cotacao = DefaultObjects.ObtemCotacaoDeMaterialPadrao();
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12, 25);
            Assert.AreEqual(1, cotacao.Impostos.Count);
            Imposto imposto = cotacao.Impostos.First();
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
            var cotacao = new CotacaoMaterial(DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                      DefaultObjects.ObtemIncotermPadrao(), "INC", 100, null, 100,DateTime.Today,null);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17, 17);
            
        }

        [TestMethod]
        public void QuandoInformoUmaCotacaoComImpostosOvalorLiquidoECalculadoCorretamente()
        {
            var cotacao = new CotacaoMaterial(DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                      DefaultObjects.ObtemIncotermPadrao(), "INC", 100, 20, 100, DateTime.Today, null);

            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12, 12);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17, 17);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 5, 5);
            Assert.AreEqual(66, cotacao.ValorLiquido);

        }
        [TestMethod]
        public void ConsigoFazerCastEmUmaCotacaoDeMaterial()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     ",", 100, 110, 120, DateTime.Today, "");

            foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
            {
                var cotacao = (CotacaoMaterial) fornecedorParticipante.Cotacao;
                Assert.IsNotNull(cotacao.CondicaoDePagamento);
            }

        }
        [TestMethod]
        public void ConsigoFazerCastEmUmaCotacaoDeFrete()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processo.AdicionarFornecedor(fornecedor);
            processo.Abrir();

            Cotacao cotacaoDeFrete =  processo.InformarCotacao(fornecedor.Codigo,1000,100,"");
            processo.SelecionarCotacao(cotacaoDeFrete.Id,150);

            foreach (var fornecedorParticipante in processo.FornecedoresParticipantes)
            {
                var cotacao = (CotacaoDeFrete)fornecedorParticipante.Cotacao;
                Assert.IsTrue(cotacao.Selecionada);
            }

        }

        
    }
}
