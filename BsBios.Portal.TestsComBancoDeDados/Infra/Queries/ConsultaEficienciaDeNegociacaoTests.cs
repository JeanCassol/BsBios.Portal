using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Queries
{
    [TestClass]
    public class ConsultaEficienciaDeNegociacaoTests
    {
        [TestMethod]
        public void QuandoConsultaEficienciaNegociacaoRetornaEficienciaDosProcessosFechados()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();
            //cria dois compradores
            Usuario comprador1 = DefaultObjects.ObtemCompradorDeSuprimentos();
            Usuario comprador2 = DefaultObjects.ObtemCompradorDeSuprimentos();

            //cria um processo aberto e um fechado para o comprador1
            ProcessoDeCotacaoDeMaterial processo11 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador1);
            var cotacaoMaterial11 = processo11.InformarCotacao(processo11.FornecedoresParticipantes.Single().Fornecedor.Codigo,
                                       DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                       DefaultObjects.ObtemIncotermPadrao(), "incoterm 2");

            var cotacaoItem11 = cotacaoMaterial11.InformarCotacaoDeItem(processo11.Itens.Single(), 100, 0, 50, DateTime.Today.AddDays(5), "obs11");
            cotacaoItem11.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12);
            cotacaoItem11.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 10);
            cotacaoItem11.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, 9);

            var processoDeCotacaoItem11 = (ProcessoDeCotacaoDeMaterialItem) processo11.Itens.Single();

            //cotacaoItem11.Atualizar(95,50,"obs11");
            cotacaoItem11.Selecionar(50);
            processo11.Fechar("texto cabeçalho","nota de cabeçalho");

            ProcessoDeCotacaoDeMaterial processo12 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador1);

            //cria um processo aberto e um fechado para o comprador2
            ProcessoDeCotacaoDeMaterial processo21 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador2);
            var cotacaoMaterial21 = processo21.InformarCotacao(processo21.FornecedoresParticipantes.Single().Fornecedor.Codigo,
                                       DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                       DefaultObjects.ObtemIncotermPadrao(), "incoterm 2");

            var cotacaoItem21 = cotacaoMaterial21.InformarCotacaoDeItem(processo21.Itens.Single(), 110, 0, 50, DateTime.Today.AddDays(5), "obs21");
            cotacaoItem21.InformarImposto(Enumeradores.TipoDeImposto.Icms, 12);
            cotacaoItem21.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 10);
            cotacaoItem21.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, 9);

            cotacaoItem21.Atualizar(99,50,"obs21");

            cotacaoItem21.Selecionar(50);

            processo21.Fechar("texto cabeçalho", "nota de cabeçalho");

            var processoDeCotacaoItem21 = (ProcessoDeCotacaoDeMaterialItem)processo21.Itens.Single();

            ProcessoDeCotacaoDeMaterial processo22 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador2);

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>
                {
                    processo11,processo12,processo21,processo22
                });


            var consulta = ObjectFactory.GetInstance<IConsultaEficienciaDeNegociacao>();
            KendoGridVm kendoGridVm = consulta.ConsultarResumo(DefaultObjects.ObtemPaginacaoDefault(),  new EficienciaNegociacaoFiltroVm());
            //retorna um registro para cada comprador
            Assert.AreEqual(3, kendoGridVm.QuantidadeDeRegistros);

            var eficiencias = kendoGridVm.Registros.Cast<EficienciaDeNegociacaoResumoVm>().ToList();

            var eficienciaDoComprador1 = eficiencias.Single(x => x.Comprador == comprador1.Nome);
            Assert.AreEqual(processo11.Id, eficienciaDoComprador1.IdProcessoCotacao);
            Assert.AreEqual(processoDeCotacaoItem11.Id, eficienciaDoComprador1.IdProcessoCotacaoItem);
            Assert.AreEqual(cotacaoItem11.ProcessoDeCotacaoItem.Produto.Descricao,eficienciaDoComprador1.Produto);
            Assert.AreEqual(processoDeCotacaoItem11.RequisicaoDeCompra.Numero, eficienciaDoComprador1.NumeroDaRequisicao);
            Assert.AreEqual(processoDeCotacaoItem11.RequisicaoDeCompra.NumeroItem, eficienciaDoComprador1.NumeroDoItem);
            Assert.AreEqual(0, eficienciaDoComprador1.ValorDeEficiencia);
            Assert.AreEqual(0, eficienciaDoComprador1.PercentualDeEficiencia);

            var eficienciaDoComprador2 = eficiencias.Single(x => x.Comprador == comprador2.Nome);
            Assert.AreEqual(processo21.Id, eficienciaDoComprador2.IdProcessoCotacao);
            Assert.AreEqual(processoDeCotacaoItem21.Id, eficienciaDoComprador2.IdProcessoCotacaoItem);
            Assert.AreEqual(cotacaoItem21.ProcessoDeCotacaoItem.Produto.Descricao, eficienciaDoComprador2.Produto);
            Assert.AreEqual(processoDeCotacaoItem21.RequisicaoDeCompra.Numero, eficienciaDoComprador2.NumeroDaRequisicao);
            Assert.AreEqual(processoDeCotacaoItem21.RequisicaoDeCompra.NumeroItem, eficienciaDoComprador2.NumeroDoItem);
            Assert.AreEqual(550, eficienciaDoComprador2.ValorDeEficiencia);
            Assert.AreEqual(10, eficienciaDoComprador2.PercentualDeEficiencia);

            var eficienciaTotal = eficiencias.Single(x => x.Produto == "TOTAL");

            Assert.IsNotNull(eficienciaTotal);
            Assert.AreEqual(550, eficienciaTotal.ValorDeEficiencia);
            Assert.AreEqual((decimal) 5.24, eficienciaTotal.PercentualDeEficiencia);

        }

    }
}
