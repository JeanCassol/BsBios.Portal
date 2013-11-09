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
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Codigo, processoConsultado.Produto.Codigo);
            Assert.AreEqual(processoDeCotacaoDeMaterial.UnidadeDeMedida.CodigoInterno, processoConsultado.UnidadeDeMedida.CodigoInterno);
            Assert.IsFalse(NHibernateUtil.IsInitialized(processoConsultado.RequisicaoDeCompra));
            Assert.AreEqual(processoDeCotacaoDeMaterial.RequisicaoDeCompra.Id, processoConsultado.RequisicaoDeCompra.Id);
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


            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc", 100, 120, 100,DateTime.Today.AddMonths(1),"obs fornec");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();


            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeMaterial)processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();
            Assert.AreEqual(processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count(x => x.Cotacao != null), processoConsultado.FornecedoresParticipantes.Count(x => x.Cotacao != null));
            Console.WriteLine("Consultando Cotacao - FIM");
        }

        [TestMethod]
        public void ConsigoPersistirEConsultarUmProcessoDeCotacaoComImpostosNasCotacoes()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);

            processoDeCotacaoDeMaterial.Abrir();
            Cotacao cotacao = processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc", 100, 120, 100, DateTime.Today.AddMonths(1), "obs fornec");

            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17, 34);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 5, 13);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();

            Console.WriteLine("Consultando Cotacao - INICIO");
            var processoConsultado = (ProcessoDeCotacaoDeMaterial)processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();
            Cotacao cotacaoConsultada = processoConsultado.FornecedoresParticipantes.First().Cotacao;
            Assert.AreEqual(2, cotacaoConsultada.Impostos.Count);
            Console.WriteLine("Consultando Cotacao - FIM");
            
        }

        [TestMethod]
        public void FiltrarUmProcessoDeCotacaoPorCodigoDoProdutoRetornaProcessoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();            

            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto1, municipios.First(), municipios.Last());
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto2, municipios.First(), municipios.Last());

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao2);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacao.CodigoDoProdutoContendo(produto1.Codigo).List();

            Assert.AreEqual(1, processosConsultados.Count());
            Assert.AreEqual(produto1.Codigo, processosConsultados.First().Produto.Codigo);
        }

        [TestMethod]
        public void FiltrarUmProcessoDeCotacaoPorDescricaoDoProdutoRetornaProcessoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();            

            Produto produto1 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto1, municipios.First(), municipios.Last());
            Produto produto2 = DefaultObjects.ObtemProdutoPadrao();
            ProcessoDeCotacaoDeFrete processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComProdutoEspecifico(produto2, municipios.First(), municipios.Last());

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao2);

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacao.DescricaoDoProdutoContendo(produto2.Descricao).List();

            Assert.AreEqual(1, processosConsultados.Count());
            Assert.AreEqual(produto2.Descricao, processosConsultados.First().Produto.Descricao);

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
