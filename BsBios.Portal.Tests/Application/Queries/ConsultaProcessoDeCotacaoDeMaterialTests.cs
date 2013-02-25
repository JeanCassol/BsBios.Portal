using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeMaterialTests
    {
        [TestMethod]
        public void GetDescriptionFromEnum()
        {
            Assert.AreEqual("Não Iniciado", Enumeradores.StatusPedidoCotacao.NaoIniciado.Descricao());
        }

        [TestMethod]
        public void ConsultaRetornaOObjetoEsperado()
        {
            var consultaProcesso = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeMaterial>();
            KendoGridVm kendoGridVm = consultaProcesso.Listar(new PaginacaoVm() { Page = 1, PageSize = 10, Take = 10}, null);

            Assert.AreEqual(1, kendoGridVm.QuantidadeDeRegistros);
            ProcessoCotacaoMaterialListagemVm  processoListagem = kendoGridVm.Registros.Cast<ProcessoCotacaoMaterialListagemVm>().First();
            Assert.AreEqual(15, processoListagem.Id);
            Assert.AreEqual("PROD0001", processoListagem.CodigoMaterial);
            Assert.AreEqual("Produto de Teste", processoListagem.Material);
            Assert.AreEqual(new Decimal(100.258), processoListagem.Quantidade);
            Assert.AreEqual("Não Iniciado", processoListagem.Status);
            Assert.IsNull(processoListagem.DataTermino);
        }
    }
}
