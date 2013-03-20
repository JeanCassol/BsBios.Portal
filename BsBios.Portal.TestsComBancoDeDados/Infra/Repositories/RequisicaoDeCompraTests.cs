using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class RequisicaoDeCompraTests: RepositoryTest
    {
        private static IRequisicoesDeCompra _requisicoesDeCompra;

        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
            _requisicoesDeCompra = ObjectFactory.GetInstance<IRequisicoesDeCompra>();
        }
        [TestMethod]
        public void DepoisDePersistirUmaRequisicaoDeCompraConsigoConsultar()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicaoDeCompra);

            UnitOfWorkNh.Session.Clear();
            
            RequisicaoDeCompra requisicaoConsultada = _requisicoesDeCompra.BuscaPeloId(requisicaoDeCompra.Id);

            Assert.IsNotNull(requisicaoConsultada);

            Assert.AreEqual(requisicaoDeCompra.Criador.Login, requisicaoConsultada.Criador.Login);
            Assert.AreEqual("requisitante", requisicaoConsultada.Requisitante);
            Assert.AreEqual(requisicaoDeCompra.FornecedorPretendido.Codigo, requisicaoConsultada.FornecedorPretendido.Codigo);
            Assert.AreEqual(requisicaoDeCompra.Material.Codigo, requisicaoConsultada.Material.Codigo);
            Assert.AreEqual(DateTime.Today.AddDays(-2), requisicaoConsultada.DataDeRemessa);
            Assert.AreEqual(DateTime.Today.AddDays(-1), requisicaoConsultada.DataDeLiberacao);
            Assert.AreEqual(DateTime.Today, requisicaoConsultada.DataDeSolicitacao);
            Assert.AreEqual("C001", requisicaoConsultada.Centro);
            Assert.AreEqual(requisicaoDeCompra.UnidadeMedida.CodigoInterno, requisicaoConsultada.UnidadeMedida.CodigoInterno);
            Assert.AreEqual(1000, requisicaoConsultada.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoConsultada.Descricao);
            Assert.AreEqual(requisicaoDeCompra.Numero, requisicaoConsultada.Numero);
            Assert.AreEqual(requisicaoDeCompra.NumeroItem, requisicaoConsultada.NumeroItem);            
        }

        [TestMethod]
        public void ConsigoPersistirEConsultarUmaRequisicaoDeCompraSemInformarRequisitanteEFornecedorPretendido()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraSemRequisitanteEFornecedor();
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicaoDeCompra);

            UnitOfWorkNh.Session.Clear();
            RequisicaoDeCompra requisicaoConsultada = _requisicoesDeCompra.BuscaPeloId(requisicaoDeCompra.Id);

            Assert.IsNotNull(requisicaoConsultada);

            Assert.AreEqual(requisicaoDeCompra.Criador.Login, requisicaoConsultada.Criador.Login);
            Assert.IsNull(requisicaoConsultada.Requisitante);
            Assert.IsNull(requisicaoConsultada.FornecedorPretendido);
            Assert.AreEqual(requisicaoDeCompra.Material.Codigo, requisicaoConsultada.Material.Codigo);
            Assert.AreEqual(DateTime.Today.AddDays(-2), requisicaoConsultada.DataDeRemessa);
            Assert.AreEqual(DateTime.Today.AddDays(-1), requisicaoConsultada.DataDeLiberacao);
            Assert.AreEqual(DateTime.Today, requisicaoConsultada.DataDeSolicitacao);
            Assert.AreEqual("C001", requisicaoConsultada.Centro);
            Assert.AreEqual(requisicaoDeCompra.UnidadeMedida.CodigoInterno, requisicaoConsultada.UnidadeMedida.CodigoInterno);
            Assert.AreEqual(1000, requisicaoConsultada.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoConsultada.Descricao);
            Assert.AreEqual("REQ0001", requisicaoConsultada.Numero);
            Assert.AreEqual("00001", requisicaoConsultada.NumeroItem);
        }

    }
}
