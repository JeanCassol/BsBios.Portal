using System;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
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

            var kendoGridVm = consultaRequisicao.Listar(DefaultObjects.ObtemPaginacaoDefault(),new RequisicaoDeCompraFiltroVm());

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
        }

    }
}
