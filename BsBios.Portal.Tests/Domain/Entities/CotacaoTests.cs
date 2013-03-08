using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
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
            var cotacao = new Cotacao(condicaoDePagamento, incoterm, "Descrição do Incoterm", 100, 110, 5);
            Assert.IsFalse(cotacao.Selecionada);
            Assert.IsNull(cotacao.Iva);
            Assert.IsNull(cotacao.QuantidadeAdquirida);
            Assert.AreEqual(100, cotacao.ValorLiquido);
            Assert.AreEqual(110, cotacao.ValorComImpostos);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm",cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(5, cotacao.Mva);
        }

        [TestMethod]
        public void QuandoAtualizaoUmaCotacaoAsPropriedadesSaoCriadasCorretamente()
        {
            Cotacao cotacao = DefaultObjects.ObtemCotacaoPadrao();
            var condicaoDePagamento = new CondicaoDePagamento("C1","CONDIÇÃO ALTERAÇÃO");
            var incoterm = new Incoterm("I1", "INCOTERM ALTERAÇÃO");
            cotacao.Atualizar(200, 220,condicaoDePagamento, incoterm,"INCOTERM ALTERADO",10 );
            Assert.AreEqual(200, cotacao.ValorLiquido);
            Assert.AreEqual(220, cotacao.ValorComImpostos);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("INCOTERM ALTERADO", cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
            Assert.AreEqual(10, cotacao.Mva);
        }

        [TestMethod]
        public void QuandoInformoUmImpostoParaACotacaoOImpostoEAdicionadoCorretamente()
        {
            Cotacao cotacao = DefaultObjects.ObtemCotacaoPadrao();
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
            Cotacao cotacao = DefaultObjects.ObtemCotacaoPadrao();
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12, 25);
            Assert.AreEqual(1, cotacao.Impostos.Count);
            Imposto imposto = cotacao.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(12, imposto.Aliquota);
            Assert.AreEqual(25, imposto.Valor);
        }

        [TestMethod]
        [ExpectedException(typeof(ValorTotalComImpostosObrigatorioException))]
        public void QuandoAdicionoImpostoEmUmaCotacaoQueNaoPossuiValorTotalComImpostosDeveGerarExcecao()
        {
            var cotacao = new Cotacao(DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                      DefaultObjects.ObtemIncotermPadrao(), "INC", 100, null, null);

            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 17);

        }

        [TestMethod]
        [ExpectedException(typeof (MvaNaoInformadoException))]
        public void QuandoACotacaoPossuiIcmsDeSubstituicaoTributariaEoCampoMvaNaoForPreenchidoDeveDispararExcecao()
        {
            var cotacao = new Cotacao(DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                      DefaultObjects.ObtemIncotermPadrao(), "INC", 100, 120, null);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17, 17);
            
        }

        
    }
}
