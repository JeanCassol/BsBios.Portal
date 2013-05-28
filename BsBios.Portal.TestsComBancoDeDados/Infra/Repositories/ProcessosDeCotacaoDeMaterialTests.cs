using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class ProcessosDeCotacaoDeMaterialTests: RepositoryTest
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
        public void DepoisDePersistirUmProcessoDeCotacaoDeMaterialConsigoConsultar()
        {
            var processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            var processoConsultado = (ProcessoDeCotacaoDeMaterial) processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();

            Assert.IsNotNull(processoConsultado);
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processoConsultado.Status);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id ,processoConsultado.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno, processoConsultado.DataLimiteDeRetorno);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Requisitos, processoConsultado.Requisitos);
            Assert.IsNull(processoConsultado.TextoDeCabecalho);

            var item = (ProcessoDeCotacaoDeMaterialItem)processoDeCotacaoDeMaterial.Itens.First();
            var itemConsultado = (ProcessoDeCotacaoDeMaterialItem) processoConsultado.Itens.First();
            Assert.AreEqual(item.Produto.Codigo, itemConsultado.Produto.Codigo);
            Assert.AreEqual(item.UnidadeDeMedida.CodigoInterno, itemConsultado.UnidadeDeMedida.CodigoInterno);
            Assert.IsFalse(NHibernateUtil.IsInitialized(itemConsultado.RequisicaoDeCompra));
            Assert.AreEqual(item.RequisicaoDeCompra.Id, itemConsultado.RequisicaoDeCompra.Id);

        }

        //Este teste verifica se quando salvo um processo de cotação, os fornecedores adicionados também são salvos e podem ser consultados
        //posteriormente
        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            //sou obrigado a executar o método Clear aqui, caso contrário o objeto que foi persistido fica em cache
            UnitOfWorkNh.Session.Clear();
            var processoConsultado = (ProcessoDeCotacaoDeMaterial)processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();

            Assert.AreEqual(processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count, processoConsultado.FornecedoresParticipantes.Count);
        }


        /// <summary>
        /// Este teste verifica se quando salvo um processo de cotação, as cotações adicionadas também são salvas
        /// e podem ser consultadas posteriormente
        /// </summary>
        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComCotacoes()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);

            var usuarioComprador = DefaultObjects.ObtemUsuarioPadrao();
            processoDeCotacaoDeMaterial.Abrir(usuarioComprador);
            var cotacao = processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc");
            var processoDeCotacaoItem = processoDeCotacaoDeMaterial.Itens.First();
            var cotacaoItem = cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 1000, 120, 100, DateTime.Today.AddMonths(1), "obs fornec");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeMaterial)processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();
            Assert.AreEqual(processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count(x => x.Cotacao != null), processoConsultado.FornecedoresParticipantes.Count(x => x.Cotacao != null));
            var cotacaoConsultada = (CotacaoMaterial) processoConsultado.FornecedoresParticipantes.First().Cotacao.CastEntity();
            Assert.AreEqual(cotacao.Incoterm, cotacaoConsultada.Incoterm.CastEntity());
            Assert.AreEqual(cotacao.DescricaoIncoterm, cotacaoConsultada.DescricaoIncoterm);
            Assert.AreEqual(cotacao.CondicaoDePagamento, cotacaoConsultada.CondicaoDePagamento.CastEntity());
            Assert.AreEqual(processoDeCotacaoDeMaterial.Comprador.Login, processoConsultado.Comprador.Login);

            var cotacaoItemConsultada = (CotacaoMaterialItem) cotacaoConsultada.Itens.First().CastEntity();

            Assert.AreEqual(cotacaoItem.ValorComImpostos, cotacaoItemConsultada.ValorComImpostos);
            Assert.AreEqual(cotacaoItem.Preco, cotacaoItemConsultada.Preco);
            Assert.AreEqual(cotacaoItem.PrecoInicial, cotacaoItemConsultada.PrecoInicial);
            Assert.AreEqual(cotacaoItem.Selecionada, cotacaoItemConsultada.Selecionada);
            Assert.AreEqual(cotacaoItem.QuantidadeDisponivel, cotacaoItemConsultada.QuantidadeDisponivel);
            Assert.AreEqual(cotacaoItem.QuantidadeAdquirida, cotacaoItemConsultada.QuantidadeAdquirida);
            Assert.AreEqual(cotacaoItem.Observacoes, cotacaoItemConsultada.Observacoes);

            Console.WriteLine("Consultando Cotacao - FIM");
        }

        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoComNotaETextoDeCabecalho()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao);

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            var processoDeCotacaoConsultado = (ProcessoDeCotacaoDeMaterial) processosDeCotacao.BuscaPorId(processoDeCotacao.Id).Single();
            Assert.AreEqual("texto de cabeçalho", processoDeCotacaoConsultado.TextoDeCabecalho);
            Assert.AreEqual("nota de cabeçalho", processoDeCotacaoConsultado.NotaDeCabecalho);

        }

        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComImpostosNasCotacoes()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);

            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            var cotacao = processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),DefaultObjects.ObtemIncotermPadrao(), "inc");
            var processoCotacaoItem = processoDeCotacaoDeMaterial.Itens.First();
            var cotacaoItem = cotacao.InformarCotacaoDeItem(processoCotacaoItem, 100, 120, 100, DateTime.Today.AddMonths(1), "obs fornec");
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 5, 13);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeMaterial)processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();
            Cotacao cotacaoConsultada = processoConsultado.FornecedoresParticipantes.First().Cotacao;
            Assert.AreEqual(2, cotacaoConsultada.Itens.First().Impostos.Count);
            Console.WriteLine("Consultando Cotacao - FIM");
            
        }


        [TestMethod]
        public void FiltrarUmProcessoDeCotacaoPorStatusRetornaProcessoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao2);

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacao.FiltraPorStatus(Enumeradores.StatusProcessoCotacao.NaoIniciado).List();

            Assert.AreEqual(1, processosConsultados.Count());
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processosConsultados.First().Status);

        }

    }
}
