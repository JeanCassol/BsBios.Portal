using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CalculadorDeBaseDeCalculoTests
    {
        [TestMethod]
        public void QuandoCalculoBaseDeCalculoDeUmImpostoPadraoRetornaOPrecoDoItem()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            CotacaoItem cotacaoItem = processoDeCotacao.FornecedoresParticipantes.Single().Cotacao.Itens.Single();
            var calculador = new CalculadorDeBaseDeCalculoPadrao();
            decimal baseDeCalculo = calculador.Calcular(cotacaoItem);
            Assert.AreEqual(cotacaoItem.Preco, baseDeCalculo);
        }

        [TestMethod]
        public void QuandoCalculaBaseDeCalculoDeUmImpostoComRetencaoRetornaValorCorreto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialDeMateriaPrima();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            CotacaoMaterial cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                              DefaultObjects.ObtemIncotermPadrao(), "descriçao");
            var processoDeCotacaoDeMaterialItem = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.Single();
            CotacaoItem cotacaoItem = cotacao.InformarCotacaoDeItem(processoDeCotacaoDeMaterialItem, 100, 0, 10,DateTime.Today.AddDays(10), "obs");
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, (decimal) 9.25);
            var calculador = new CalculadorDeBaseDeCalculoComCreditoDeImpostos();
            decimal baseDeCalculo = calculador.Calcular(cotacaoItem);
            Assert.AreEqual((decimal) 78.75, baseDeCalculo);
            
        }
    }
}
