using System;
using System.Linq;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaQuotaTests: RepositoryTest
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
        public void QuandoConsultaQuotaEmUmaDataQuePossuiQuotaRetornaVerdadeiro()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000",DateTime.Today,1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsTrue(consultaQuota.PossuiQuotaNaData(DateTime.Today));
           
        }
        [TestMethod]
        public void QuandoConsultaQuotaEmUmaDataQueNaoPossuiQuotaRetornaFalso()
        {
            //RemoveQueries.RemoverQuotasCadastradas();
            var quota = new Quota(Enumeradores.MaterialDeCarga.Soja, DefaultObjects.ObtemTransportadoraPadrao(),
                "1000", DateTime.Today, 1200);
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            Assert.IsFalse(consultaQuota.PossuiQuotaNaData(DateTime.Today.AddDays(1)));
            
        }

        [TestMethod]
        public void QuandoConsultaQuotasEmUmaDeterminadaDataRetornaListaDeQuotasDeTodosOsFornecedoresDaquelaData()
        {
            RemoveQueries.RemoverQuotasCadastradas();
            Quota quota = DefaultObjects.ObtemQuotaDeCarregamento();
            DefaultPersistedObjects.PersistirQuota(quota);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            IList<QuotaConsultarVm> quotas = consultaQuota.QuotasDaData(quota.Data);

            Assert.AreEqual(1, quotas.Count);
        }

        [TestMethod]
        public void QuandoConsultaQuotasDeUmDeterminadoFornecedorRetornaListaDeQuotasEmOrdemDecrescenteDeData()
        {
            //cria duas quotas para o mesmo fornecedor
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            var quota1 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor, "1000",DateTime.Today,100);
            var quota2 = new Quota(Enumeradores.MaterialDeCarga.Farelo, fornecedor, "1000", DateTime.Today.AddDays(1), 100);

            DefaultPersistedObjects.PersistirQuota(quota1);
            DefaultPersistedObjects.PersistirQuota(quota2);

            var consultaQuota = ObjectFactory.GetInstance<IConsultaQuota>();
            KendoGridVm kendoGridVm =
                consultaQuota.ListarQuotasDoFornecedor(new PaginacaoVm {Page = 1, PageSize = 10, Take = 10},fornecedor.Codigo);

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            IList<QuotaPorFornecedorVm> quotas = kendoGridVm.Registros.Cast<QuotaPorFornecedorVm>().ToList();
            Assert.IsTrue(quotas[0].Data > quotas[1].Data);
        }
    }
}
