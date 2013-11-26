using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.TestsComBancoDeDados.Infra;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaOrdemDeTransporteTests : RepositoryTest
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
        public void QuandoListoOrdensDeTransporteRetornaDadosEsperados()
        {

            RemoveQueries.RemoverOrdensDeTransporteCadastradas();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());

            IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso();

            var ordemDeTransporte = ordensDeTransporte.First();

            DefaultPersistedObjects.PersistirOrdensDeTransporte(ordensDeTransporte, processoDeCotacao);

            var consultaOrdemDeTransporte = ObjectFactory.GetInstance<IConsultaOrdemDeTransporte>();

            var paginacao = new PaginacaoVm
            {
                Page = 1,
                Take = 10,
                PageSize = 10
            };

            FornecedorParticipante participanteSelecionado = processoDeCotacao.FornecedoresSelecionados.First();

            Console.WriteLine("Inicio da consulta");
            var filtro = new OrdemDeTransporteListagemFiltroVm
            {
                NomeDoFornecedor = "Fornec"
            };
            var kendoGridVm = consultaOrdemDeTransporte.Listar(paginacao, filtro);
            Console.WriteLine("Fim da consulta");

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);

            Fornecedor fornecedorSelecionado = participanteSelecionado.Fornecedor;

            var ordemDeTransporteVm = kendoGridVm.Registros.Cast<OrdemDeTransporteListagemVm>().First();

            Assert.AreEqual(processoDeCotacao.Produto.Descricao, ordemDeTransporteVm.Material);
            Assert.AreEqual(fornecedorSelecionado.Codigo, ordemDeTransporteVm.CodigoDoFornecedor);
            Assert.AreEqual(fornecedorSelecionado.Nome, ordemDeTransporteVm.NomeDoFornecedor);
            Assert.AreEqual(ordemDeTransporte.QuantidadeColetada, ordemDeTransporteVm.QuantidadeColetada);
            Assert.AreEqual(ordemDeTransporte.QuantidadeLiberada, ordemDeTransporteVm.QuantidadeLiberada);

        }

        [TestMethod]
        public void ConsigoListarMonitor()
        {
            RemoveQueries.RemoverOrdensDeTransporteCadastradas();

            IList<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());

            IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso();

            DefaultPersistedObjects.PersistirOrdensDeTransporte(ordensDeTransporte, processoDeCotacao);

            var consultaOrdemDeTransporte = ObjectFactory.GetInstance<IConsultaMonitorDeOrdemDeTransporte>();
            var filtro = new MonitorDeOrdemDeTransporteFiltroVm
            {
                DataInicial = DateTime.Today.AddMonths(1).ToShortDateString(),
                DataFinal = DateTime.Today.AddMonths(2).ToShortDateString()
            };
            IList<MonitorDeOrdemDeTransporteVm> dados = consultaOrdemDeTransporte.Listar(filtro);

            Assert.AreEqual(1, dados.Count);

            MonitorDeOrdemDeTransporteVm registro = dados.Single();

            Assert.AreEqual(9, registro.QuantidadeLiberada);
        }

        //[TestMethod]
        //public void ConsigoListarMonitor2()
        //{
        //    RemoveQueries.RemoverOrdensDeTransporteCadastradas();

        //    IList<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

        //    ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());

        //    IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso();

        //    DefaultPersistedObjects.PersistirOrdensDeTransporte(ordensDeTransporte, processoDeCotacao);

        //    var consultaOrdemDeTransporte = ObjectFactory.GetInstance<IConsultaOrdemDeTransporte>();
        //    var filtro = new MonitorDeOrdemDeTransporteFiltroVm
        //    {
        //        DataInicial = DateTime.Today.AddMonths(1),
        //        DataFinal = DateTime.Today.AddMonths(2)
        //    };
        //    IList<MonitorDeOrdemDeTransporteVm> dados = consultaOrdemDeTransporte.ListagemDoMonitor2(filtro);

        //    Assert.AreEqual(1, dados.Count);

        //    MonitorDeOrdemDeTransporteVm registro = dados.Single();

        //    Assert.AreEqual(9, registro.QuantidadeLiberada);
        //}


        [TestMethod]
        public void ConsigoConsultarQuantidadeLiberadaEmTodasAsOrdensDeTransporteDeUmProcessoDeCotacao()
        {
            List<Municipio> municipiosCadastrados = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipiosCadastrados.First(), municipiosCadastrados.Last());

            IList<OrdemDeTransporte> ordemDeTransportes = processoDeCotacao.FecharProcesso();

            DefaultPersistedObjects.PersistirOrdensDeTransporte(ordemDeTransportes, processoDeCotacao);

            var consulta = ObjectFactory.GetInstance<IConsultaOrdemDeTransporte>();

            var quantidade = consulta.CalcularQuantidadeLiberadaPeloProcessoDeCotacao(processoDeCotacao.Id);

            Assert.AreEqual(processoDeCotacao.FornecedoresSelecionados.Sum(x => x.Cotacao.QuantidadeAdquirida), quantidade);
        }
    }
}
