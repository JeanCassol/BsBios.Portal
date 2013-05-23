using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
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
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            ProcessoDeCotacao processoConsultado = processosDeCotacao.BuscaPorId(processo.Id).Single();
            Assert.IsNotNull(processoConsultado);
            Assert.AreEqual(1, processoConsultado.Itens.Count);
        }

        /// <summary>
        /// Este teste verifica se quando salvo um processo de cotação, as cotações adicionadas também são salvas
        /// e podem ser consultadas posteriormente
        /// </summary>
        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComCotacoes()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processo.AdicionarFornecedor(fornecedor);

            processo.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            processo.InformarCotacao(fornecedor.Codigo, 120, 100, "obs fornec");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeFrete)processosDeCotacaoDeMaterial.BuscaPorId(processo.Id).Single();
            Assert.AreEqual(processo.FornecedoresParticipantes.Count(x => x.Cotacao != null), processoConsultado.FornecedoresParticipantes.Count(x => x.Cotacao != null));
            Console.WriteLine("Consultando Cotacao - FIM");
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
