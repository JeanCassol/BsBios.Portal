using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class RequisicoesDeCompraTests: RepositoryTest
    {
        [TestMethod]
        public void QuandoBuscaRequisicoesDeCompraDisponiveisParaUmProcessoDeCotacaoRetornaTodasRequisicoesDeCompraQueAindaNaoGeraramProcesso()
        {
            RemoveQueries.RemoverRequisicoesDeCompraCadastradas();
            RequisicaoDeCompra requisicao1 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            RequisicaoDeCompra requisicao2 = DefaultObjects.ObtemRequisicaoDeCompraPadrao();

            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao1);
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(requisicao2);

            var requisicoesDeCompra = ObjectFactory.GetInstance<IRequisicoesDeCompra>();

            var requisicoesConsultadas = requisicoesDeCompra.DisponiveisParaProcessoDeCotacao(1).List();
            Assert.AreEqual(2, requisicoesConsultadas.Count);

        }
    }
}
