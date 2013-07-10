using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Queries.Builders;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Infra.Builders
{
    [TestClass]
    public class FornecedorCadastroBuilderTests
    {
        [TestMethod]
        public void BuilderSingleCriarAViewModelComAsPropriedadesCorretas()
        {
            var builder = new FornecedorCadastroBuilder();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            FornecedorCadastroVm  viewModel = builder.BuildSingle(fornecedor);
            Assert.AreEqual(fornecedor.Codigo, viewModel.Codigo);
            Assert.AreEqual(fornecedor.Nome, viewModel.Nome);
            Assert.AreEqual(fornecedor.Email, viewModel.Email);
        }
        [TestMethod]
        public void BuilderListChamaOMetodoSingleParaCadaUmDosFornecedoresDaLista()
        {
            var builder = new FornecedorCadastroBuilder();

            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            var viewModels = builder.BuildList(new List<Fornecedor>()
                {
                    fornecedor1, fornecedor2
                });

            Assert.AreEqual(2, viewModels.Count);
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor1.Codigo));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == fornecedor2.Codigo));
        }

    }
}
