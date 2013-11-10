using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
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
            ProcessoDeCotacao processoDeCotacao = processosDeCotacao.BuscaPorId(processo.Id).Single();
            Assert.IsNotNull(processoDeCotacao);

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

            processo.Abrir();
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

    }
}
