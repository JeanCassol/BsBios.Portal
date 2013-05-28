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
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm,"Descrição do Incoterm");
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm",cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
        }

        [TestMethod]
        public void QuandoCrioUmItemDeCotacaoAsPropriedasSaoIniciadasCorretamente()
        {
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var fornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm, "Descrição do Incoterm");
            var itemDoProcesso = processoDeCotacao.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem) cotacao.InformarCotacaoDeItem(itemDoProcesso, 110,5, 80, DateTime.Today.AddMonths(1), "observacoes");

            Assert.IsFalse(cotacaoItem.Selecionada);
            Assert.IsNull(cotacaoItem.Iva);
            Assert.IsNull(cotacaoItem.QuantidadeAdquirida);
            Assert.AreEqual(110, cotacaoItem.Preco);
            Assert.AreEqual(110, cotacaoItem.PrecoInicial);
            Assert.AreEqual(110, cotacaoItem.ValorComImpostos);
            Assert.AreEqual(110, cotacaoItem.Custo);
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
            cotacao.Atualizar(condicaoDePagamento, incoterm, "INCOTERM ALTERADO");
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("INCOTERM ALTERADO", cotacao.DescricaoIncoterm);
            Assert.AreSame(condicaoDePagamento, cotacao.CondicaoDePagamento);
        }

        [TestMethod]
        public void QuandoAtualizoUmItemDeCotacaoAsPropriedadesSaoAlteradasCorretamente()
        {
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            var fornecedor = processoDeCotacao.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor, condicaoDePagamento, incoterm, "Descrição do Incoterm");
            var itemDoProcesso = processoDeCotacao.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem)cotacao.InformarCotacaoDeItem(itemDoProcesso, 110, 0, 80, DateTime.Today.AddMonths(1), "observacoes");

            decimal valorLiquidoInicial = cotacaoItem.Preco;
            cotacaoItem.Atualizar(220, 10,90, DateTime.Today.AddMonths(2), "observacoes alteradas");

            Assert.AreEqual(220, cotacaoItem.Preco);
            Assert.AreEqual(valorLiquidoInicial, cotacaoItem.PrecoInicial);
            Assert.AreEqual(220, cotacaoItem.ValorComImpostos);
            Assert.AreEqual(220, cotacaoItem.Custo);
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
            
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17);
            Assert.AreEqual(1, cotacaoItem.Impostos.Count);
            Imposto imposto = cotacaoItem.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(17, imposto.Aliquota);
            Assert.AreEqual((decimal) 21.25, imposto.Valor);
        }

        [TestMethod]
        public void QuandoUmImpostoJaExistenteeEInformadoNovamenteOMesmoEAtualizado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacaoItem = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12);
            Assert.AreEqual(1, cotacaoItem.Impostos.Count);
            Imposto imposto = cotacaoItem.Impostos.First();
            Assert.AreEqual(Enumeradores.TipoDeImposto.Icms, imposto.Tipo);
            Assert.AreEqual(12, imposto.Aliquota);
            Assert.AreEqual(15, imposto.Valor);
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
            var cotacao = processoDeCotacao.InformarCotacao(codigoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),DefaultObjects.ObtemIncotermPadrao(), "INC");
            var cotacaoItem = cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 100, null, 100, DateTime.Today, null);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17);
        }

        [TestMethod]
        public void QuandoInformoUmaCotacaoComImpostosOvalorComImpostoECalculadoCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var cotacaoItem = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Itens.First();

            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 10);
            //o preço da cotação padrão é 125. O Valor do IPI é 12,5 (10%)
            //valor com impostos = preço + valor do ipi = 125 + 12,50
            Assert.AreEqual((decimal) 137.5, cotacaoItem.ValorComImpostos);
        }

        [TestMethod]
        public void QuandoInformoUmaCotacaoParaUmaCotacaoParaUmProdutoQueNaoEMateriaPrimaCalculadoCustoCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            CotacaoMaterial cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                              DefaultObjects.ObtemIncotermPadrao(), "incoterm");

            ProcessoDeCotacaoItem item = processoDeCotacao.Itens.Single();
            var cotacaoItem = cotacao.InformarCotacaoDeItem(item, 100, 0, 10, DateTime.Today.AddDays(10), "obs");
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, 9);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 10);
            //preço + ipi
            Assert.AreEqual(110, cotacaoItem.Custo);
            
        }

        [TestMethod]
        public void QuandoInformoUmaCotacaoParaUmaMateriaPrimaCalculadoCustoCorretamente()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialDeMateriaPrima();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            CotacaoMaterial cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                              DefaultObjects.ObtemIncotermPadrao(), "incoterm");

            ProcessoDeCotacaoItem item = processoDeCotacao.Itens.Single();
            var cotacaoItem = cotacao.InformarCotacaoDeItem(item, 100, 0, 10, DateTime.Today.AddDays(10), "obs");
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, 9);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 10);
            
            //PREÇO - ICMS - PIS / COFINS - IPI = 100 - 17 - 9 - 7,40
            Assert.AreEqual((decimal) 66.6, cotacaoItem.Custo);

        }

    }
}
