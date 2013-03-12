using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
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

        //[TestMethod]
        //public void QueryPersonalizada()
        //{
        //    //var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
        //    var queryable = UnitOfWorkNh.Session.Query<ProcessoDeCotacao>();
        //    IList<ProdutoCadastroVm> produtos = (from pcm in queryable //queryPcm
        //                 where pcm.Id == 5
        //                 && pcm is ProcessoDeCotacaoDeMaterial
        //                 let processo = (ProcessoDeCotacaoDeMaterial)pcm
        //                 select new ProdutoCadastroVm()
        //                 {
        //                     CodigoSap = processo.RequisicaoDeCompra.Material.Codigo,
        //                     Descricao = processo.RequisicaoDeCompra.Material.Descricao,
        //                     Tipo = processo.RequisicaoDeCompra.Material.Tipo,
        //                 }).ToList();
            
        //    Assert.IsTrue(produtos.Count > 0);

        //}

        [TestMethod]
        public void DepoisDePersistirUmProcessoDeCotacaoDeMaterialConsigoConsultar()
        {
            var processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            //DefaultPersistedObjects.PersistirRequisicaoDeCompra(processoDeCotacaoDeMaterial.RequisicaoDeCompra);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            //UnitOfWorkNh.BeginTransaction();
            //var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            //processosDeCotacaoDeMaterial.Save(processoDeCotacaoDeMaterial);
            //UnitOfWorkNh.Commit();

            UnitOfWorkNh.Session.Clear();

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            var processoConsultado = (ProcessoDeCotacaoDeMaterial) processosDeCotacaoDeMaterial.BuscaPorId(processoDeCotacaoDeMaterial.Id).Single();

            Assert.IsNotNull(processoConsultado);
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processoConsultado.Status);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id ,processoConsultado.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno, processoConsultado.DataLimiteDeRetorno);
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
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);


            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc", 100, 120, null);

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
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();

            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();

            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor);

            processoDeCotacaoDeMaterial.Abrir();
            Cotacao cotacao = processoDeCotacaoDeMaterial.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc", 100, 120, null);

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


    }
}
