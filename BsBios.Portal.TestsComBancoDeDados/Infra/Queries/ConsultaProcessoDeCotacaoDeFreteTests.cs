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
    public class ConsultaProcessoDeCotacaoDeFreteTests
    {
        [TestMethod]
        public void ConsigoConsultarOsDadosDeUmProcesso()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeFrete>();

            ProcessoCotacaoFreteCadastroVm viewModel = consulta.ConsultaProcesso(processo.Id);

            Assert.IsNotNull(viewModel);
        }

        [TestMethod]
        public void QuandoConsultaCotacaoDeFornecedoresQueNaoInformaramCotacaoRetornaDadosEsperados()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComFornecedor();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            FornecedorParticipante fornecedorParticipante = processo.FornecedoresParticipantes.Single();
            Fornecedor fornecedor = fornecedorParticipante.Fornecedor;


            var consulta = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeFrete>();

            IList<CotacaoSelecionarVm> cotacoes = consulta.CotacoesDosFornecedores(processo.Id);
            Assert.AreEqual(1, cotacoes.Count);
            var cotacaoSelecionarVm = cotacoes.Single();
            Assert.AreEqual(fornecedor.Cnpj,cotacaoSelecionarVm.Cnpj);
            Assert.AreEqual(fornecedor.Nome, cotacaoSelecionarVm.Fornecedor);
            Assert.AreEqual(fornecedor.Codigo,cotacaoSelecionarVm.CodigoFornecedor);
            Assert.AreEqual(0, cotacaoSelecionarVm.IdCotacao);
            Assert.AreEqual(0, cotacaoSelecionarVm.IdProcessoCotacaoItem);
            Assert.IsFalse(cotacaoSelecionarVm.Selecionada);
            Assert.IsNull(cotacaoSelecionarVm.QuantidadeAdquirida);
            Assert.IsNull(cotacaoSelecionarVm.QuantidadeDisponivel);
            Assert.IsNull(cotacaoSelecionarVm.ValorComImpostos);
            Assert.IsNull(cotacaoSelecionarVm.ObservacaoDoFornecedor);
        }

        [TestMethod]
        public void ConsigoConsultarCotacaoResumidaDosFornecedoresDeUmProcessoDeCotacaoDeFrete()
        {

            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoNaoSelecionada();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);
            FornecedorParticipante fornecedorParticipante = processo.FornecedoresParticipantes.Single();
            Fornecedor fornecedor = fornecedorParticipante.Fornecedor;
            CotacaoItem cotacaoItem = fornecedorParticipante.Cotacao.Itens.Single();

            var consulta = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeFrete>();
            KendoGridVm kendoGridVm = consulta.CotacoesDosFornecedoresResumido(processo.Id);
            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoFornecedorVm vm = kendoGridVm.Registros.Cast<ProcessoCotacaoFornecedorVm>().Single();
            Assert.AreEqual(fornecedor.Codigo, vm.Codigo);
            Assert.AreEqual(fornecedor.Nome, vm.Nome);
            Assert.AreEqual(fornecedorParticipante.Id,vm.IdFornecedorParticipante);
            Assert.AreEqual(cotacaoItem.QuantidadeDisponivel,vm.QuantidadeDisponivel);
            Assert.AreEqual("Não",vm.Selecionado);
            Assert.AreEqual(cotacaoItem.ValorComImpostos,vm.ValorComImpostos);
            Assert.AreEqual(cotacaoItem.Preco, vm.ValorLiquido);

        }

    }
}
