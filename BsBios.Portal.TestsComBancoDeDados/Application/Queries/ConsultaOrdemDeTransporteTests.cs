using System;
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
    }
}
