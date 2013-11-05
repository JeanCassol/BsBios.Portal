using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class OrdensDeTransporteTests:RepositoryTest
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
        public void ConsigoPersistirEConsultarUmaOrdemDeTransporte()
        {
            UnitOfWorkNh.BeginTransaction();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());
            OrdemDeTransporte ordemDeTransporte = processoDeCotacao.FecharProcesso().First();

            var ordensDeTransporte = ObjectFactory.GetInstance<IOrdensDeTransporte>();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao);
            ordensDeTransporte.Save(ordemDeTransporte);

            UnitOfWorkNh.Commit();

            OrdemDeTransporte ordemDeTransporteConsultada = ordensDeTransporte.BuscaPorId(ordemDeTransporte.Id);

            Assert.AreEqual(ordemDeTransporte.Id, ordemDeTransporteConsultada.Id);
            Assert.AreSame(ordemDeTransporte.Fornecedor, ordemDeTransporteConsultada.Fornecedor);
            Assert.AreEqual(ordemDeTransporte.QuantidadeAdquirida, ordemDeTransporteConsultada.QuantidadeAdquirida);
            Assert.AreEqual(ordemDeTransporte.QuantidadeLiberada, ordemDeTransporteConsultada.QuantidadeLiberada);

        }
    }
}
