using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
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
            Queries.RemoverRequisicoesDeCompraCadastradas();

            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            DefaultPersistedObjects.PersistirUsuarios(new List<Usuario>(){requisicaoDeCompra.Criador, requisicaoDeCompra.Requisitante});
            DefaultPersistedObjects.PersistirFornecedor(requisicaoDeCompra.FornecedorPretendido);
            DefaultPersistedObjects.PersistirProduto(requisicaoDeCompra.Material);

            UnitOfWorkNh.BeginTransaction();

            _requisicoesDeCompra.Save(requisicaoDeCompra);
            UnitOfWorkNh.Commit();
            
            UnitOfWorkNh.Session.Clear();
            RequisicaoDeCompra requisicaoConsultada = _requisicoesDeCompra.BuscaPeloId(requisicaoDeCompra.Id);

            Assert.IsNotNull(requisicaoConsultada);

            Assert.AreEqual("criador", requisicaoConsultada.Criador.Login);
            Assert.AreEqual("requisitante", requisicaoConsultada.Requisitante.Login);
            Assert.AreEqual("fpret", requisicaoConsultada.FornecedorPretendido.Codigo);
            Assert.AreEqual("MAT0001", requisicaoConsultada.Material.Codigo);
            Assert.AreEqual(DateTime.Today.AddDays(-2), requisicaoConsultada.DataDeRemessa);
            Assert.AreEqual(DateTime.Today.AddDays(-1), requisicaoConsultada.DataDeLiberacao);
            Assert.AreEqual(DateTime.Today, requisicaoConsultada.DataDeSolicitacao);
            Assert.AreEqual("C001", requisicaoConsultada.Centro);
            Assert.AreEqual("UNT", requisicaoConsultada.UnidadeMedida);
            Assert.AreEqual(1000, requisicaoConsultada.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoConsultada.Descricao);
            Assert.AreEqual("REQ0001", requisicaoConsultada.Numero);
            Assert.AreEqual("00001", requisicaoConsultada.NumeroItem);            
        }
    }
}
