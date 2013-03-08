using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
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
            Assert.IsNull(vm.ValorComImpostos);
            Assert.IsNull(vm.ValorLiquido);
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
            Assert.AreEqual(120, vm.ValorComImpostos);
            Assert.AreEqual(100, vm.ValorLiquido);
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
            Assert.AreEqual(120, vm.ValorComImpostos);
            Assert.AreEqual(100, vm.ValorLiquido);
            Assert.AreEqual(12, vm.Mva);
            Assert.AreEqual("Aberto", vm.Status);
            
        }

        [TestMethod]
        public void QuandoInformoImpostosDeUmaCotacaoRetornaOsDadosDosImpostos()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            Cotacao cotacao = processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm", 100, 120, 12);

            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Icms, 1,2);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 11, 12);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 21, 22);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Pis, 31, 32);
            cotacao.InformarImposto(Enumeradores.TipoDeImposto.Cofins, 41, 42);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoCadastroVm vm = consulta.ConsultarCotacao(processo.Id, fornecedor.Codigo);

            Assert.AreEqual(1, vm.IcmsAliquota);
            Assert.AreEqual(2, vm.IcmsValor);
            Assert.AreEqual(11, vm.IcmsStAliquota);
            Assert.AreEqual(12, vm.IcmsStValor);
            Assert.AreEqual(21, vm.IpiAliquota);
            Assert.AreEqual(22, vm.IpiValor);
            Assert.AreEqual(31, vm.PisAliquota);
            Assert.AreEqual(32, vm.PisValor);
            Assert.AreEqual(41, vm.CofinsAliquota);
            Assert.AreEqual(42, vm.CofinsValor);

            
        }



    }


}
