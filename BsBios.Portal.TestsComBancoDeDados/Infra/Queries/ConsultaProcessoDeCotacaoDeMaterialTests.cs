using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeMaterialTests: RepositoryTest
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
        public void ConsultaProcessoRetornaObjetoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            ProcessoCotacaoMaterialCadastroVm processoCotacaoMaterialCadastroVm = consultaProcesso.ConsultaProcesso(processoDeCotacaoDeMaterial.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoCotacaoMaterialCadastroVm.Id);
            //Assert.AreEqual(processoDeCotacaoDeMaterial.Itens.First().Produto.Codigo,processoCotacaoMaterialCadastroVm.CodigoMaterial);
            Assert.IsTrue(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoCotacaoMaterialCadastroVm.DataLimiteRetorno);
            Assert.AreEqual("Não Iniciado",processoCotacaoMaterialCadastroVm.DescricaoStatus);
            Assert.AreEqual("Requisitos do Processo de Cotação de Materiais", processoCotacaoMaterialCadastroVm.Requisitos);
        }

        [TestMethod]
        public void ConsultaListagemDeProcessosRetornaObjetoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10}, 
                new ProcessoCotacaoFiltroVm());

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoMaterialListagemVm  processoListagem = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().First();
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoListagem.Id);
            //Assert.AreEqual(processoDeCotacaoDeMaterial.Itens.First().Produto.Codigo, processoListagem.CodigoMaterial);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Itens.First().Produto.Descricao, processoListagem.Material);
            //Assert.AreEqual(1000, processoListagem.Quantidade);
            Assert.AreEqual("Não Iniciado", processoListagem.Status);
            Assert.IsNotNull(processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoListagem.DataTermino);
        }

        [TestMethod]
        public void QuandoConsultaListagemDeUmProcessoQueNaoPossuiItensRetornaObjetoEsperado()
        {
            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();

            var processoDeCotacaoDeMaterial = new ProcessoDeCotacaoDeMaterial();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today, "requisitos");
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10 },
                new ProcessoCotacaoFiltroVm());

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoMaterialListagemVm processoListagem = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().First();
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoListagem.Id);
            Assert.AreEqual("Sem Materiais", processoListagem.Material);
            Assert.AreEqual("Não Iniciado", processoListagem.Status);
            Assert.IsNotNull(processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoListagem.DataTermino);
            
        }

        [TestMethod]
        public void QuandoListaProcessosDeCotacaoDeUmDeterminadoFornecedorRetornaApenasProcessosDesteFornecedor()
        {
            //crio dois fornecedores e adiciono cada um deles em duas cotações distintas
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao3 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao4 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            processoDeCotacao1.AdicionarFornecedor(fornecedor1);
            processoDeCotacao1.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            processoDeCotacao2.AdicionarFornecedor(fornecedor1);
            processoDeCotacao2.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            processoDeCotacao3.AdicionarFornecedor(fornecedor2);
            processoDeCotacao3.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            
            processoDeCotacao4.AdicionarFornecedor(fornecedor2);
            processoDeCotacao4.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>()
                {processoDeCotacao1, processoDeCotacao2, processoDeCotacao3, processoDeCotacao4});

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            //consulta filtrando pelo fornecedor1
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10 }, 
               new ProcessoCotacaoFiltroVm()
                   {
                       CodigoFornecedor  = fornecedor1.Codigo
                   });
            Assert.AreEqual(2,kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().ToList();
            //verifico que está retornado os dois processos vinculados ao fornecedor 1
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao1.Id));
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao2.Id));
        }

        [TestMethod]
        public void QuandoConsultaProcessosDeCotacaoDeUmDeterminadoFornecedorNaoConsideraOsProcessosNaoIniciados()
        {
            //crio um fornecedor e adiciono ele em uma cotação aberta e uma não iniciada
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacao1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            ProcessoDeCotacaoDeMaterial processoDeCotacao2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            processoDeCotacao1.AdicionarFornecedor(fornecedor1);
            processoDeCotacao1.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            processoDeCotacao2.AdicionarFornecedor(fornecedor1);

            DefaultPersistedObjects.PersistirProcessosDeCotacaoDeMaterial(new List<ProcessoDeCotacaoDeMaterial>() { processoDeCotacao1, processoDeCotacao2});

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            //consulta filtrando pelo fornecedor
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10 },
               new ProcessoCotacaoFiltroVm()
               {
                   CodigoFornecedor = fornecedor1.Codigo
               });
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().ToList();
            //verifico que está retornado apenas o processo que foi aberto
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao1.Id));
            
        }

        [TestMethod]
        public void ConsultaFornecedoresParticipantesRetornaObjetoEsperado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);

            Console.WriteLine("INICIANDO CONSULTA");
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.FornecedoresParticipantes(processoDeCotacaoDeMaterial.Id);
            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            IList<FornecedorCadastroVm> viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor1.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor2.Codigo));

        }

        [TestMethod]
        public void QuandoConsultaCotacaoResumidaAntesDoFornecedorInformarCotacaoRetornaObjetoEsperado()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            FornecedorParticipante fornecedorParticipante = processoDeCotacao.AdicionarFornecedor(fornecedor);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao);

            UnitOfWorkNh.Session.Clear();

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.CotacoesDosFornecedoresResumido(processoDeCotacao.Id);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var processoCotacaoFornecedorVm = (ProcessoCotacaoFornecedorVm) kendoGridVm.Registros.First();
            
            Assert.AreEqual(fornecedorParticipante.Id, processoCotacaoFornecedorVm.IdFornecedorParticipante);
            Assert.AreEqual(fornecedor.Codigo, processoCotacaoFornecedorVm.Codigo);
            Assert.AreEqual(fornecedor.Nome, processoCotacaoFornecedorVm.Nome);
            Assert.AreEqual("Não",processoCotacaoFornecedorVm.Selecionado);
            Assert.IsNull(processoCotacaoFornecedorVm.ValorLiquido);
            Assert.IsNull(processoCotacaoFornecedorVm.ValorComImpostos);
            Assert.IsNull(processoCotacaoFornecedorVm.QuantidadeDisponivel);
            Assert.AreEqual("Não", processoCotacaoFornecedorVm.VisualizadoPeloFornecedor);
        }

        [TestMethod]
        public void QuandoConsultaCotacaoResumidaAposFornecedorInformarCotacaoRetornaObjetoEsperado()
        {
            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada();
            FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresParticipantes.First();
            Fornecedor fornecedor = fornecedorParticipante.Fornecedor;
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao);

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.CotacoesDosFornecedoresResumido(processoDeCotacao.Id);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var processoCotacaoFornecedorVm = (ProcessoCotacaoFornecedorVm)kendoGridVm.Registros.First();

            Assert.AreEqual(fornecedorParticipante.Id, processoCotacaoFornecedorVm.IdFornecedorParticipante);
            Assert.AreEqual(fornecedor.Codigo, processoCotacaoFornecedorVm.Codigo);
            Assert.AreEqual(fornecedor.Nome, processoCotacaoFornecedorVm.Nome);
            Assert.AreEqual("Sim", processoCotacaoFornecedorVm.Selecionado);
            Assert.AreEqual("Não", processoCotacaoFornecedorVm.VisualizadoPeloFornecedor);
        }

        [TestMethod]
        public void ConsigoConsultarItensDeUmProcessoDeCotacaoDeMaterial()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao);

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.ListarItens(processoDeCotacao.Id);
            IList<ProcessoDeCotacaoDeMaterialItemVm> itens = kendoGridVm.Registros.Cast<ProcessoDeCotacaoDeMaterialItemVm>().ToList();

            Assert.IsNotNull(itens);
            Assert.AreEqual(1, itens.Count);
            var requisicaoDeCompraVm = itens.First();
            var item = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.First();
            Assert.AreEqual(item.RequisicaoDeCompra.Numero, requisicaoDeCompraVm.NumeroRequisicao);
            Assert.AreEqual(item.RequisicaoDeCompra.NumeroItem,requisicaoDeCompraVm.NumeroItem);

        }

        [TestMethod]
        public void ConsigoConsultarAsCotacoesDetalhadasDeTodosOsFornecedores()
        {
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
            //crio um processo de cotação com um item e três fornecedores.
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();

            ProcessoDeCotacaoItem processoDeCotacaoItem = processoDeCotacao.Itens.Single();

            processoDeCotacao.AdicionarFornecedor(fornecedor1);
            processoDeCotacao.AdicionarFornecedor(fornecedor2);
            processoDeCotacao.AdicionarFornecedor(fornecedor3);

            processoDeCotacao.Abrir(DefaultObjects.ObtemCompradorDeSuprimentos());

            //para os fornecedores 1 e 3 informo o preço duas vezes
            CotacaoMaterial cotacaoFornecedor1 = processoDeCotacao.InformarCotacao(fornecedor1.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                DefaultObjects.ObtemIncotermPadrao(), "inc");

            CotacaoItem cotacaoItemFornecedor1 = cotacaoFornecedor1.InformarCotacaoDeItem(processoDeCotacaoItem, 1000, 0, 200, DateTime.Today.AddDays(10), "OBS");
            cotacaoFornecedor1.InformarCotacaoDeItem(processoDeCotacaoItem, 995, 0, 200, DateTime.Today.AddDays(10), "OBS");

            CotacaoMaterial cotacaoFornecedor3 = processoDeCotacao.InformarCotacao(fornecedor3.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                DefaultObjects.ObtemIncotermPadrao(), "inc");

            cotacaoFornecedor3.InformarCotacaoDeItem(processoDeCotacaoItem, 1010, 0, 100, DateTime.Today.AddDays(10), "OBS");
            cotacaoFornecedor3.InformarCotacaoDeItem(processoDeCotacaoItem, 1000, 0, 100, DateTime.Today.AddDays(10), "OBS");

            //para o fornecedor 2 informo o preço apenas uma vez
            CotacaoMaterial cotacaoFornecedor2 = processoDeCotacao.InformarCotacao(fornecedor2.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                DefaultObjects.ObtemIncotermPadrao(), "inc");

            cotacaoFornecedor2.InformarCotacaoDeItem(processoDeCotacaoItem, 1200, 0, 100, DateTime.Today.AddDays(10), "OBS");

            //seleciono apenas o fornecedor 1
            cotacaoItemFornecedor1.Selecionar(200);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao);

            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            IList<FornecedorCotacaoVm> cotacoes = consultaProcesso.CotacoesDetalhadaDosFornecedores(processoDeCotacao.Id, processoDeCotacaoItem.Id);

            Assert.AreEqual(3, cotacoes.Count);

            //asserts fornecedor 1
            FornecedorCotacaoVm fornecedorCotacaoVm1 = cotacoes.Single(x => x.Codigo == fornecedor1.Codigo);
            Assert.AreEqual(1000, fornecedorCotacaoVm1.PrecoInicial);
            Assert.AreEqual(995, fornecedorCotacaoVm1.PrecoFinal);
            Assert.IsTrue(fornecedorCotacaoVm1.Selecionada);
            Assert.AreEqual(200, fornecedorCotacaoVm1.QuantidadeAdquirida);
            //Assert.AreEqual(2, fornecedorCotacaoVm1.Precos.Length);
            Assert.AreEqual(2, fornecedorCotacaoVm1.Precos.Count());

            Assert.AreEqual(1000, fornecedorCotacaoVm1.Precos[0]);
            Assert.AreEqual(995, fornecedorCotacaoVm1.Precos[1]);

            //asserts fornecedor 2
            FornecedorCotacaoVm fornecedorCotacaoVm2 = cotacoes.Single(x => x.Codigo == fornecedor2.Codigo);
            Assert.AreEqual(1200, fornecedorCotacaoVm2.PrecoInicial);
            Assert.AreEqual(1200, fornecedorCotacaoVm2.PrecoFinal);
            Assert.IsFalse(fornecedorCotacaoVm2.Selecionada);
            Assert.AreEqual(0, fornecedorCotacaoVm2.QuantidadeAdquirida);
            //Assert.AreEqual(1, fornecedorCotacaoVm2.Precos.Length);
            Assert.AreEqual(1, fornecedorCotacaoVm2.Precos.Count());

            Assert.AreEqual(1200, fornecedorCotacaoVm2.Precos[0]);

            //asserts fornecedor 3
            FornecedorCotacaoVm fornecedorCotacaoVm3 = cotacoes.Single(x => x.Codigo == fornecedor3.Codigo);
            Assert.AreEqual(1010, fornecedorCotacaoVm3.PrecoInicial);
            Assert.AreEqual(1000, fornecedorCotacaoVm3.PrecoFinal);
            Assert.IsFalse(fornecedorCotacaoVm3.Selecionada);
            Assert.AreEqual(0, fornecedorCotacaoVm3.QuantidadeAdquirida);
            //Assert.AreEqual(2, fornecedorCotacaoVm3.Precos.Length);
            Assert.AreEqual(2, fornecedorCotacaoVm3.Precos.Count());

            Assert.AreEqual(1010, fornecedorCotacaoVm3.Precos[0]);
            Assert.AreEqual(1000, fornecedorCotacaoVm3.Precos[1]);

        }

        [TestMethod]
        public void RetornaListaComTodosFornecedoresParticipantesDoProcessoDeCotacao()
        {
            //crio um processo de cotação de materiais
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(4), "req");

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor1);
            processoDeCotacao.AdicionarFornecedor(fornecedor2);
            processoDeCotacao.AdicionarFornecedor(fornecedor3);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacao);

            var consulta = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();

            FornecedorVm[] fornecedores = consulta.ListarFornecedores(processoDeCotacao.Id);

            Assert.AreEqual(3, fornecedores.Count());

            Assert.IsTrue(fornecedores.Any(x => x.Codigo == fornecedor1.Codigo && x.Nome == fornecedor1.Nome));
            Assert.IsTrue(fornecedores.Any(x => x.Codigo == fornecedor2.Codigo && x.Nome == fornecedor2.Nome));
            Assert.IsTrue(fornecedores.Any(x => x.Codigo == fornecedor3.Codigo && x.Nome == fornecedor3.Nome));

        }


    }
}
