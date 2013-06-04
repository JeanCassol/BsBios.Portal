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
            Assert.AreEqual("N�o Iniciado",processoCotacaoMaterialCadastroVm.DescricaoStatus);
            Assert.AreEqual("Requisitos do Processo de Cota��o de Materiais", processoCotacaoMaterialCadastroVm.Requisitos);
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
            Assert.AreEqual("N�o Iniciado", processoListagem.Status);
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
            Assert.AreEqual("N�o Iniciado", processoListagem.Status);
            Assert.IsNotNull(processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
            Assert.AreEqual(processoDeCotacaoDeMaterial.DataLimiteDeRetorno.Value.ToShortDateString(), processoListagem.DataTermino);
            
        }

        [TestMethod]
        public void QuandoListaProcessosDeCotacaoDeUmDeterminadoFornecedorRetornaApenasProcessosDesteFornecedor()
        {
            //crio dois fornecedores e adiciono cada um deles em duas cota��es distintas
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
            //verifico que est� retornado os dois processos vinculados ao fornecedor 1
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao1.Id));
            Assert.IsNotNull(viewModels.First(x => x.Id == processoDeCotacao2.Id));
        }

        [TestMethod]
        public void QuandoConsultaProcessosDeCotacaoDeUmDeterminadoFornecedorNaoConsideraOsProcessosNaoIniciados()
        {
            //crio um fornecedor e adiciono ele em uma cota��o aberta e uma n�o iniciada
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
            //verifico que est� retornado apenas o processo que foi aberto
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
            Assert.AreEqual("N�o",processoCotacaoFornecedorVm.Selecionado);
            Assert.IsNull(processoCotacaoFornecedorVm.ValorLiquido);
            Assert.IsNull(processoCotacaoFornecedorVm.ValorComImpostos);
            Assert.IsNull(processoCotacaoFornecedorVm.QuantidadeDisponivel);
            Assert.AreEqual("N�o", processoCotacaoFornecedorVm.VisualizadoPeloFornecedor);
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
            Assert.AreEqual("N�o", processoCotacaoFornecedorVm.VisualizadoPeloFornecedor);
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

    }
}
