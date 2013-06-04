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
    public class ConsultaRequisicaoDeCompraTests
    {
        [TestMethod]
        public void ConsigoListarAsRequisicoesDeCompra()
        {
            RemoveQueries.RemoverRequisicoesDeCompraCadastradas();

            RequisicaoDeCompra requisicao1 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            RequisicaoDeCompra requisicao2 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();

            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao1);
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao2);

            var consultaRequisicao = ObjectFactory.GetInstance<IConsultaRequisicaoDeCompra>();

            var kendoGridVm = consultaRequisicao.Listar(DefaultObjects.ObtemPaginacaoDefault(),new RequisicaoDeCompraFiltroVm(10));

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
        }

        [TestMethod]
        public void QuandoUmaRequisicaoDeCompraJaGerouUmProcessoDeCotacaoNaoApareceMaisDisponivelParaOutrosProcessos()
        {
            //Cria duas requisi��es de compra: 1 e 2
            //Cria um processo de cota��o e associa a requisi��o 1 com este processo
            //Cria um novo processo de cota��o, salva e Lista as requisi��es para este processo. A requisi��o ' n�o deve aparecer.
            RemoveQueries.RemoverRequisicoesDeCompraCadastradas();

            RequisicaoDeCompra requisicao1 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            RequisicaoDeCompra requisicao2 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();

            var processo1 = new ProcessoDeCotacaoDeMaterial();
            processo1.AdicionarItem(requisicao1);
            var processo2 = new ProcessoDeCotacaoDeMaterial();

            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao2);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo1);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo2);

            var consultaRequisicao = ObjectFactory.GetInstance<IConsultaRequisicaoDeCompra>();

            var kendoGridVm = consultaRequisicao.Listar(DefaultObjects.ObtemPaginacaoDefault(), new RequisicaoDeCompraFiltroVm(processo2.Id));

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            var requisicaoDeCompraVm = kendoGridVm.Registros.Cast<RequisicaoDeCompraVm>().Single();

            Assert.AreEqual(requisicao2.Numero, requisicaoDeCompraVm.NumeroRequisicao);
            Assert.AreEqual(requisicao2.NumeroItem, requisicaoDeCompraVm.NumeroItem);

        }

        [TestMethod]
        public void QuandoConsultoRequisicoesDisponiveisParaUmProcessoDeCotacaoListaAsRequisicoesQueJaEstaoAssociadasAoProcessoETambemAsQueNaoEstaoAssociadasANenhumOutroProcesso()
        {
            //Cria duas requisi��es de compra: 1 e 2
            //Cria um processo de cota��o 1 e associa a requisi��o 1 com este processo
            //Salva os registros
            //Depois consultar novamente as requisi��es para o processo 1: deve listar as requisi�es 1 e 2
            RemoveQueries.RemoverRequisicoesDeCompraCadastradas();

            RequisicaoDeCompra requisicao1 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            RequisicaoDeCompra requisicao2 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();

            var processo1 = new ProcessoDeCotacaoDeMaterial();
            processo1.AdicionarItem(requisicao1);

            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao1);
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao2);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo1);

            var consultaRequisicao = ObjectFactory.GetInstance<IConsultaRequisicaoDeCompra>();

            var kendoGridVm = consultaRequisicao.Listar(DefaultObjects.ObtemPaginacaoDefault(), new RequisicaoDeCompraFiltroVm(processo1.Id));

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);

        }

    }
}
