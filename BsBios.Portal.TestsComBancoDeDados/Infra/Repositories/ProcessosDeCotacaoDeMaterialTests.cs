using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Script.Serialization;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();

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
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 17);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 5);

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

        [TestMethod]
        public void RepositorioDoProcessoDeCotacaoDeMateriasContemApenasProcessosDeCotacaoDeMaterial()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();
            //crio dois processos de cotação (um de frete e um de materiais) e persisto
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            ProcessoDeCotacaoDeFrete processoDeCotacaoDeFrete = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacaoDeFrete);

            //listo todos os processsos do repositório: deve retornar apenas o processo de cotação de  material
            var processosDeCotacaoDeMaterias = ObjectFactory.GetInstance<IProcessosDeCotacaoDeMaterial>();
            var todosProcessos = processosDeCotacaoDeMaterias.List();
            Assert.AreEqual(1, todosProcessos.Count);
            Assert.IsInstanceOfType(todosProcessos.Single(), typeof(ProcessoDeCotacaoDeMaterial));
        }

        [TestMethod]
        public void QuandoBuscoProcessosDeCotacaoDeUmCompradorEspecificoTodosProcessosSaoDesteComprador()
        {
            //crio dois processos de cotação: 1 para cada comprador
            Usuario comprador1 = DefaultObjects.ObtemUsuarioPadrao();
            comprador1.AdicionarPerfil(Enumeradores.Perfil.CompradorSuprimentos);
            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador1);

            Usuario comprador2 = DefaultObjects.ObtemUsuarioPadrao();
            comprador2.AdicionarPerfil(Enumeradores.Perfil.CompradorSuprimentos);
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAberto(comprador2);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao2);

            var processosDeCotacaoDeMateriais = ObjectFactory.GetInstance<IProcessosDeCotacaoDeMaterial>();
            IList<ProcessoDeCotacao> processosConsultados = processosDeCotacaoDeMateriais.EfetuadosPeloComprador(comprador1.Login).List();

            Assert.AreEqual(1, processosConsultados.Count);

            Assert.AreEqual(processoDeCotacao1.Id, processosConsultados.Single().Id);

        }

        [TestMethod]
        public void QuandoFiltroProcessosDeCotacaoPorRequisicaoDeCompraRetornaApenasProcessosVinculadosComARequisicao()
        {
            //crio duas requisicoes de compra e  um processo para cada requisicão
            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            RequisicaoDeCompra requisicaoDeCompra1 = ((ProcessoDeCotacaoDeMaterialItem) processoDeCotacao1.Itens.Single()).RequisicaoDeCompra;
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>{processoDeCotacao1,processoDeCotacao2});

            //filtra os processos por uma das requisições
            var processosDeCotacao = ObjectFactory.GetInstance<IProcessosDeCotacao>();
            var processosConsultados = processosDeCotacao.GeradosPelaRequisicaoDeCompra(requisicaoDeCompra1.Numero, requisicaoDeCompra1.NumeroItem).List();

            //retorna apenas o processo vinculado com a requisição indicada
            Assert.AreEqual(1, processosConsultados.Count);
            Assert.AreEqual(processoDeCotacao1.Id, processosConsultados.Single().Id);

        }

        [TestMethod]
        public void TesteRelatorioDinamico()
        {
            IList<dynamic> listaDinamica = FornecedoresDinamicos.RetornoDinamico();
            foreach (dynamic o in listaDinamica)
            {
                Assert.AreEqual("Bob", o.FirstName);
                Assert.AreEqual("Smith", o.LastName);
            }

            //var serializer = new JavaScriptSerializer();
            //Console.WriteLine(serializer.Serialize(listaDinamica));

            Console.WriteLine(JsonConvert.SerializeObject( listaDinamica, new KeyValuePairConverter( ) ));


        }

    }

    internal static class FornecedoresDinamicos
    {

        public static IList<dynamic> RetornoDinamico()
        {
            dynamic data = new ExpandoObject();
            IList<dynamic> listaDinamica = new List<dynamic>();

            var dictionary = (IDictionary<string, object>)data;
            dictionary.Add("FirstName", "Bob");
            dictionary.Add("LastName", "Smith");

            listaDinamica.Add(dictionary);

            return listaDinamica;
        }
    }
}
