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
        public void QuandoConsultoUmaCotacaoDoFornecedorQueAindaNaoFoiPreenchidaRetornaOsDadosEsperados()
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
            Assert.AreEqual(processo.RequisicaoDeCompra.Descricao, vm.DescricaoDoProcessoDeCotacao);
            Assert.AreEqual(fornecedor.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(),vm.DataLimiteDeRetorno);
            Assert.AreEqual(processo.Produto.Descricao, vm.Material);
            Assert.AreEqual(processo.Quantidade, vm.Quantidade);
            Assert.AreEqual(processo.RequisicaoDeCompra.UnidadeMedida, vm.UnidadeDeMedida);
            Assert.IsNull(vm.ValorTotalComImpostos);
            Assert.IsNull(vm.ValorTotalSemImpostos);
            Assert.IsNull(vm.Mva);
            Assert.AreEqual("Aberto", vm.Status);
        }

        [TestMethod]
        public void QuandoConsultoUmaCotacaoDoFornecedorQueJaFoiPreenchidaRetornaOsDadosEsperados()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm", 100, 120, 12);
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            Cotacao cotacao = processo.FornecedoresParticipantes.First().Cotacao;

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoCadastroVm vm = consulta.ConsultarCotacao(processo.Id, fornecedor.Codigo);
            Assert.IsNotNull(vm);
            Assert.AreEqual(cotacao.CondicaoDePagamento.Codigo,  vm.CodigoCondicaoPagamento);
            Assert.AreEqual(cotacao.Incoterm.Codigo, vm.CodigoIncoterm);
            Assert.AreEqual("Desc Incoterm", vm.DescricaoIncoterm);
            Assert.AreEqual(processo.RequisicaoDeCompra.Descricao, vm.DescricaoDoProcessoDeCotacao);
            Assert.AreEqual(fornecedor.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(), vm.DataLimiteDeRetorno);
            Assert.AreEqual(processo.Produto.Descricao, vm.Material);
            Assert.AreEqual(processo.Quantidade, vm.Quantidade);
            Assert.AreEqual(processo.RequisicaoDeCompra.UnidadeMedida, vm.UnidadeDeMedida);
            Assert.AreEqual(120, vm.ValorTotalComImpostos);
            Assert.AreEqual(100, vm.ValorTotalSemImpostos);
            Assert.AreEqual(12, vm.Mva);
            Assert.AreEqual("Aberto", vm.Status);
        }

        [TestMethod]
        public void QuandoConsultaUmaCotacaoQueParticipaMaisDeUmFornecedorRetornaOsDadoDoFornecedorInformado()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2= DefaultObjects.ObtemFornecedorPadrao();
            processo.AdicionarFornecedor(fornecedor1);
            processo.AdicionarFornecedor(fornecedor2);
            processo.Abrir();

            processo.InformarCotacao(fornecedor1.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm", 100, 120, 12);

            processo.InformarCotacao(fornecedor2.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm", 120, 130, 14);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            Cotacao cotacao = processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == fornecedor1.Codigo).Cotacao;

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoCadastroVm vm = consulta.ConsultarCotacao(processo.Id, fornecedor1.Codigo);

            Assert.IsNotNull(vm);
            Assert.AreEqual(cotacao.CondicaoDePagamento.Codigo, vm.CodigoCondicaoPagamento);
            Assert.AreEqual(cotacao.Incoterm.Codigo, vm.CodigoIncoterm);
            Assert.AreEqual("Desc Incoterm", vm.DescricaoIncoterm);
            Assert.AreEqual(processo.RequisicaoDeCompra.Descricao, vm.DescricaoDoProcessoDeCotacao);
            Assert.AreEqual(fornecedor1.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(), vm.DataLimiteDeRetorno);
            Assert.AreEqual(processo.Produto.Descricao, vm.Material);
            Assert.AreEqual(processo.Quantidade, vm.Quantidade);
            Assert.AreEqual(processo.RequisicaoDeCompra.UnidadeMedida, vm.UnidadeDeMedida);
            Assert.AreEqual(120, vm.ValorTotalComImpostos);
            Assert.AreEqual(100, vm.ValorTotalSemImpostos);
            Assert.AreEqual(12, vm.Mva);
            Assert.AreEqual("Aberto", vm.Status);
            
        }

    }


}
