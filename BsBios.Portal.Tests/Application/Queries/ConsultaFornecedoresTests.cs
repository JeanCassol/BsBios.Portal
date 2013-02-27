using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaFornecedoresTests: RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }
        [TestMethod]
        public void QuandoConsultarFornecedoresDoProdutoRetornaListaDeFornecedores()
        {
            //excluindo registros existentes
            Tests.Queries.RemoverRequisicoesDeCompraCadastradas();
            Tests.Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();
            Tests.Queries.RemoverProdutosCadastrados();
            Tests.Queries.RemoverFornecedoresCadastrados();
            //preparando o cenários

            UnitOfWorkNh.BeginTransaction();
            var produto = new Produto("PROD0001", "PRODUTO 0001", "01");
            var fornecedor01 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor0001@empresa.com.br");
            var fornecedor02 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor0002@empresa.com.br");
            produto.AdicionarFornecedores(new List<Fornecedor>{fornecedor01, fornecedor02});
            UnitOfWorkNh.Session.Save(produto);
            UnitOfWorkNh.Commit();

            UnitOfWorkNh.Session.Clear();

            var consultaFornecedores = ObjectFactory.GetInstance<IConsultaFornecedor>();

            var kendoGridVm = consultaFornecedores.FornecedoresDoProduto("PROD0001");

            Assert.AreEqual(2, kendoGridVm.QuantidadeDeRegistros);
            //FIZ O ASSERT DE APENAS UM DOS FORNECEDORES PARA VERIFICAR SE A CONSTRUÇÃO DA VM ESTÁ CORRETA.
            //O MAIS CORRETO ERA FAZER UM BUILDER E TESTAR ISTO ISOLADAMENTE (O BUILDER RECEBERIA UM OBJETO DO TIPO
            //FORNECEDOR E RETORNAR UM OBJETO DO TIPO FornecedorCadastroVm.
            var fornecedor01Vm = (FornecedorCadastroVm) kendoGridVm.Registros.First();
            Assert.AreEqual("FORNEC0001",fornecedor01Vm.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedor01Vm.Nome);
            Assert.AreEqual("fornecedor0001@empresa.com.br", fornecedor01Vm.Email);

        }
    }
}
