using System;
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
        [TestMethod]
        public void DepoisDePersistirUmaRequisicaoDeCompraConsigoConsultar()
        {
            UnitOfWorkNh.BeginTransaction();
            var requisicoesDeCompra = ObjectFactory.GetInstance<IRequisicoesDeCompra>();
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            requisicoesDeCompra.Save(requisicaoDeCompra);
            UnitOfWorkNh.Commit();
            
            int idRequisicao = requisicaoDeCompra.Id;
            requisicaoDeCompra = null;
            RequisicaoDeCompra requisicaoConsultada = requisicoesDeCompra.BuscaPeloId(idRequisicao);

            Assert.IsNotNull(requisicaoConsultada);

            Assert.AreEqual("criador", requisicaoConsultada.Criador.Login);
            Assert.AreEqual("requisitante", requisicaoConsultada.Requisitante.Login);
            Assert.AreEqual("fpret", requisicaoConsultada.FornecedorPretendido.Codigo);
            Assert.AreEqual("MAT0001", requisicaoConsultada.Material.Codigo);
            Assert.AreEqual(DateTime.Today.AddDays(-2), requisicaoConsultada.DataDeRemessa);
            Assert.AreEqual(DateTime.Today.AddDays(-1), requisicaoConsultada.DataDeLiberacao);
            Assert.AreEqual(DateTime.Today, requisicaoConsultada.DataDeSolicitacao);
            Assert.AreEqual("CENTRO", requisicaoConsultada.Centro);
            Assert.AreEqual("UNT", requisicaoConsultada.UnidadeMedida);
            Assert.AreEqual(1000, requisicaoConsultada.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoConsultada.Descricao);
            Assert.AreEqual("REQ0001", requisicaoConsultada.Numero);
            Assert.AreEqual("ITEM001", requisicaoConsultada.NumeroItem);            
        }
    }

    public interface IRequisicoesDeCompra:ICompleteRepository<RequisicaoDeCompra>
    {
        RequisicaoDeCompra BuscaPeloId(int id);
    }
}
