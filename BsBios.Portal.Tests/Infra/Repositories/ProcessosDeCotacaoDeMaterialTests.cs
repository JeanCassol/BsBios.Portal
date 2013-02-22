using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Infra.Repositories
{
    [TestClass]
    public class ProcessosDeCotacaoDeMaterialTests
    {
        [TestMethod]
        public void DepoisDePersistirUmProcessoDeCotacaoDeMaterialConsigoConsultar()
        {
            Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();
            var processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            DefaultPersistedObjects.PersistirRequisicaoDeCompra(processoDeCotacaoDeMaterial.RequisicaoDeCompra);

            var processosDeCotacaoDeMaterial = ObjectFactory.GetInstance<IProcessosDeCotacaoDeMaterial>();
        }
    }
}
