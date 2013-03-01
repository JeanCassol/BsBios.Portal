using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeMaterialTests
    {
        [TestMethod]
        public void ConsultaRetornaObjetoEsperado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processoDeCotacaoDeMaterial);
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10}, null);

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoMaterialListagemVm  processoListagem = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().First();
            Assert.AreEqual(processoDeCotacaoDeMaterial.Id, processoListagem.Id);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Codigo, processoListagem.CodigoMaterial);
            Assert.AreEqual(processoDeCotacaoDeMaterial.Produto.Descricao, processoListagem.Material);
            Assert.AreEqual(1000, processoListagem.Quantidade);
            Assert.AreEqual("Não Iniciado", processoListagem.Status);
            Assert.IsNull(processoListagem.DataTermino);
        }
    }
}
