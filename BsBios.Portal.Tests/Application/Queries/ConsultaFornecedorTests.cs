using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaFornecedorTests
    {
        [TestMethod]
        public void QuandoConsultarFornecedoresNaoVinculadosRetornaListaDeFornecedores()
        {
            Fornecedor fornecedor03 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor04 = DefaultObjects.ObtemFornecedorPadrao();
            var listaFornecedores = new List<Fornecedor> {fornecedor03, fornecedor04};

            var fornecedoresMock = new Mock<IFornecedores>(MockBehavior.Strict);
            fornecedoresMock.Setup(x => x.FornecedoresNaoVinculadosAoProduto(It.IsAny<string>()))
                            .Returns(fornecedoresMock.Object);

            fornecedoresMock.Setup(x => x.Count())
                            .Returns(listaFornecedores.Count);
            fornecedoresMock.Setup(x => x.List())
                            .Returns(listaFornecedores);

            fornecedoresMock.Setup(x => x.Skip(It.IsAny<int>()))
                            .Returns(fornecedoresMock.Object);
            fornecedoresMock.Setup(x => x.Take(It.IsAny<int>()))
                            .Returns(fornecedoresMock.Object);

            var consultaFornecedores = new ConsultaFornecedor(fornecedoresMock.Object, new FornecedorCadastroBuilder(), new ProdutoCadastroBuilder());

            var paginacaoVm = new PaginacaoVm()
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };
            var kendoGridVm = consultaFornecedores.FornecedoresNaoVinculadosAoProduto(paginacaoVm, "PROD0001");

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            var viewModels = kendoGridVm.Registros.Cast<FornecedorCadastroVm>().ToList();
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor03.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor04.Codigo));

            fornecedoresMock.Verify(x => x.FornecedoresNaoVinculadosAoProduto(It.IsAny<string>()), Times.Once());
            fornecedoresMock.Verify(x => x.Count(), Times.Once());
        }

        [TestMethod]
        public void QuandoFiltraFornecedoresPorNomeRetornaListaEContabemDeRegistrosCorreta()
        {
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();
            fornecedor2.Atualizar("CARLOS EDUARDO DA SILVA", fornecedor2.Email,"","","");
            fornecedor3.Atualizar("LUIS EDUARDO SILVA", fornecedor3.Email,"","","");
            DefaultPersistedObjects.PersistirFornecedor(fornecedor1);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor2);
            DefaultPersistedObjects.PersistirFornecedor(fornecedor3);

            var consultaFornecedor = ObjectFactory.GetInstance<IConsultaFornecedor>();

            var paginacaoVm = new PaginacaoVm()
                {
                    Page = 1,
                    PageSize = 10,
                    Take = 10
                };
            KendoGridVm kendoGridVm = consultaFornecedor.Listar(paginacaoVm, "eduardo");

            Assert.AreEqual(2,kendoGridVm.QuantidadeDeRegistros);

        }


    }
}
