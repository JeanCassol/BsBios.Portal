using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaCotacaoDoFornecedorTests
    {
        [TestMethod]
        public void QuandoConsultoUmaDeterminadaCotacaoDoFornecedorRetornaOsDadosEsperados()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoCadastroVm vm = consulta.ConsultarCotacao(processo.Id, fornecedor.Codigo);
            Assert.IsNotNull(vm);
            Assert.IsNull(vm.CodigoCondicaoPagamento);
            Assert.IsNull(vm.CodigoIncoterm);
            Assert.IsNull(vm.DescricaoIncoterm);
            Assert.AreEqual("", vm.DescricaoDoProcessoDeCotacao);
            Assert.AreEqual(fornecedor.Codigo, vm.CodigoFornecedor);
            Assert.AreEqual(processo.DataLimiteDeRetorno,vm.DataLimiteDeRetorno);
            Assert.AreEqual(processo.Produto.Descricao, vm.Material);
            Assert.AreEqual(processo.Quantidade, vm.Quantidade);
            Assert.AreEqual("UND", vm.UnidadeDeMedida);
            Assert.IsNull(vm.ValorTotalComImpostos);
            Assert.IsNull(vm.ValorTotalSemImpostos);
            Assert.IsNull(vm.Mva);
            Assert.AreEqual("Aberto", vm.Status);
        }
    }
}
