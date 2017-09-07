using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class ProcessosDeCotacaoDeFreteTests:RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void ConsigoPersistirUmProcessoDeCotacaoDeFreteEConsultarPosteriormente()
        {
            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete(municipios.First(), municipios.Last());

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            var processoConsultado = (ProcessoDeCotacaoDeFrete) processosDeCotacao.BuscaPorId(processo.Id).Single();
            Assert.IsNotNull(processoConsultado);
            Assert.AreEqual(processo.Terminal.Codigo, processoConsultado.Terminal.Codigo);
            //PARA O TESTE FICAR MAIS COMPLETO DEVE SER FEITO ASSERT DE TODAS AS OUTRAS PROPRIEDADES
            Assert.AreEqual(1, processoConsultado.Itens.Count);

        }

        [TestMethod]
        public void ConsigoPersistirUmProcessoDeCotacaoFechadoEConsultarONumeroDasCondicoesGeradasNoSap()
        {
            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado(municipios.First(), municipios.Last());

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            var processoConsultado = (ProcessoDeCotacaoDeFrete)processosDeCotacao.BuscaPorId(processo.Id).Single();
            Assert.IsNotNull(processoConsultado);

            foreach (var fornecedoresSelecionado in processo.FornecedoresSelecionados)
            {
                FornecedorParticipante fornecedorSelecionadoConsultado = processoConsultado.FornecedoresSelecionados.Single(x => x.Fornecedor.Codigo == fornecedoresSelecionado.Fornecedor.Codigo);
                var cotacaoDeFrete = (CotacaoDeFrete) fornecedoresSelecionado.Cotacao;
                var cotacaoDeFreteConsultada = (CotacaoDeFrete)fornecedorSelecionadoConsultado.Cotacao.CastEntity();

                Assert.AreEqual(cotacaoDeFrete.NumeroDaCondicaoGeradaNoSap, cotacaoDeFreteConsultada.NumeroDaCondicaoGeradaNoSap);

            }
        }


        /// <summary>
        /// Este teste verifica se quando salvo um processo de cotação, as cotações adicionadas também são salvas
        /// e podem ser consultadas posteriormente
        /// </summary>
        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComCotacoes()
        {
            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete(municipios.First(), municipios.Last());

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processo.AdicionarFornecedor(fornecedor);

            processo.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            processo.InformarCotacao(fornecedor.Codigo, 120, 100, "obs fornec");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacaoDeFrete = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeFrete)processosDeCotacaoDeFrete.BuscaPorId(processo.Id).Single();
            Assert.AreEqual(processo.FornecedoresParticipantes.Count(x => x.Cotacao != null), processoConsultado.FornecedoresParticipantes.Count(x => x.Cotacao != null));
            Console.WriteLine("Consultando Cotacao - FIM");
        }

        [TestMethod]
        public void QuandoDesconsideroProcessosDeCotacaoCanceladosRetornaApenasProcessosDeCotacaoEmOutrosEstados()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();
            //criar 4 processos um em cada estado
            ProcessoDeCotacaoDeFrete processoNaoIniciado = DefaultObjects.ObtemProcessoDeCotacaoDeFrete(municipios.First(), municipios.Last());
            ProcessoDeCotacaoDeFrete processoAberto = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada(municipios.First(), municipios.Last());
            ProcessoDeCotacaoDeFrete processoFechado = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado(municipios.First(), municipios.Last());
            ProcessoDeCotacaoDeFrete processoCancelado = DefaultObjects.ObtemProcessoDeCotacaoDeFreteCancelado(municipios.First(), municipios.Last());

            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Cancelado, processoCancelado.Status);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoNaoIniciado);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoAberto);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoFechado);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoCancelado);

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            var processos = processosDeCotacao.DesconsideraCancelados().List();

            Assert.AreEqual(3, processos.Count);
            Assert.IsTrue(processos.All(x => x.Status != Enumeradores.StatusProcessoCotacao.Cancelado));

        }
        [TestMethod]
        public void FiltrarUmProcessoDeCotacaoPorCodigoDoProdutoRetornaProcessoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto1);
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto2);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao2);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacao.CodigoDoProdutoContendo(produto1.Codigo).List();

            Assert.AreEqual(1, processosConsultados.Count());
            Assert.AreEqual(produto1.Codigo, processosConsultados.First().Itens.First().Produto.Codigo);
        }

        [TestMethod]
        public void FiltrarUmProcessoDeCotacaoPorDescricaoDoProdutoRetornaProcessoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto1);
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto2);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao2);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacao.DescricaoDoProdutoContendo(produto2.Descricao).List();

            Assert.AreEqual(1, processosConsultados.Count());
            Assert.AreEqual(produto2.Descricao, processosConsultados.First().Itens.First().Produto.Descricao);

        }


    }
}
