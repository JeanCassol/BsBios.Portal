using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaFornecedorTests
    {
        [TestMethod]
        public void QuandoConsultarFornecedoresNaoVinculadosRetornaListaDeFornecedores()
        {
            var fornecedor03 = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor0003@empresa.com.br");
            var fornecedor04 = new Fornecedor("FORNEC0004", "FORNECEDOR 0004", "fornecedor0004@empresa.com.br");
            var listaFornecedores = new List<Fornecedor> {fornecedor03, fornecedor04};

            var fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            fornecedoresMock.Setup(x => x.FornecedoresNaoVinculadosAoProduto(It.IsAny<string>()))
                            .Returns(fornecedoresMock.Object);

            fornecedoresMock.Setup(x => x.Count())
                            .Returns(listaFornecedores.Count);
            fornecedoresMock.Setup(x => x.List())
                            .Returns(listaFornecedores);

            var consultaFornecedores = new ConsultaFornecedor(fornecedoresMock.Object, new FornecedorCadastroBuilder());

            var kendoGridVm = consultaFornecedores.FornecedoresNaoVinculadosAoProduto("PROD0001");

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == "FORNEC0003"));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == "FORNEC0004"));

            fornecedoresMock.Verify(x => x.FornecedoresNaoVinculadosAoProduto(It.IsAny<string>()), Times.Once());
            fornecedoresMock.Verify(x => x.Count(), Times.Once());
        }


    }
}
