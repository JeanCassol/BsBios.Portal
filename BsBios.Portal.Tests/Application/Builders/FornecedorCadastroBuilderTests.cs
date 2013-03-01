using System.Linq;
using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Builders
{
    [TestClass]
    public class FornecedorCadastroBuilderTests
    {
        [TestMethod]
        public void BuilderSingleCriarAViewModelComAsPropriedadesCorretas()
        {
            var builder = new FornecedorCadastroBuilder();
            FornecedorCadastroVm  viewModel = builder.BuildSingle(new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br"));
            Assert.AreEqual("FORNEC0001", viewModel.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", viewModel.Nome);
            Assert.AreEqual("fornecedor0001@empresa.com.br", viewModel.Email);
        }
        [TestMethod]
        public void BuilderListChamaOMetodoSingleParaCadaUmDosFornecedoresDaLista()
        {
            var builder = new FornecedorCadastroBuilder();

            var viewModels = builder.BuildList(new List<Fornecedor>()
                {
                    new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br"),
                    new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0001@empresa.com.br"),
                });

            Assert.AreEqual(2, viewModels.Count);
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == ("FORNEC0001")));
            Assert.AreEqual(1, viewModels.Count(x => x.Codigo == ("FORNEC0002")));
        }

        [TestMethod]
        public void ConsigoInstaciarOBuilderDeFornecedorCadastro()
        {
            var builder = ObjectFactory.GetInstance<IBuilder<Fornecedor, FornecedorCadastroVm>>();
            Assert.IsNotNull(builder);
            Assert.IsInstanceOfType(builder, typeof(FornecedorCadastroBuilder));
        }

    }
}
