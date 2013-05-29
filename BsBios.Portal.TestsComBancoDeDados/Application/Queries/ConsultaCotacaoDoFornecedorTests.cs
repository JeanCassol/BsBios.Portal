using System;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
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
            CotacaoMaterialCadastroVm vm = consulta.ConsultarCotacaoDeMaterial(processo.Id, fornecedor.Codigo);
            Assert.IsNotNull(vm);
            Assert.AreEqual(processo.Id, vm.IdProcessoCotacao);
            Assert.AreEqual(0, vm.IdCotacao);
            Assert.IsNull(vm.CodigoCondicaoPagamento);
            Assert.IsNull(vm.CodigoIncoterm);
            Assert.IsNull(vm.DescricaoIncoterm);
            Assert.AreEqual(fornecedor.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(),vm.DataLimiteDeRetorno);
            Assert.AreEqual("Aberto", vm.Status);
        }

        [TestMethod]
        public void QuandoConsultoUmaCotacaoDoFornecedorParaUmItemQueAindaNaoFoiPreenchidaRetornaOsDadosEsperados()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            var itemDoProcesso = (ProcessoDeCotacaoDeMaterialItem) processo.Itens.First();
            RequisicaoDeCompra requisicaoDeCompra = itemDoProcesso.RequisicaoDeCompra;

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoMaterialItemCadastroVm vm = consulta.ConsultarCotacaoDeItemDeMaterial(processo.Id, fornecedor.Codigo,requisicaoDeCompra.Numero,requisicaoDeCompra.NumeroItem);
            Assert.IsNotNull(vm);
            Assert.AreEqual(itemDoProcesso.Produto.Descricao, vm.Material);
            Assert.AreEqual(itemDoProcesso.Quantidade, vm.Quantidade);
            Assert.AreEqual(itemDoProcesso.UnidadeDeMedida.Descricao, vm.UnidadeDeMedida);
            Assert.IsNull(vm.ValorComImpostos);
            Assert.IsNull(vm.Preco);
            Assert.IsNull(vm.Mva);
            
        }

        [TestMethod]
        public void QuandoConsultoUmaCotacaoDoFornecedorQueJaFoiPreenchidaRetornaOsDadosEsperados()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            var cotacao = processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm");
            var processoCotacaoItem = processo.Itens.First();
            cotacao.InformarCotacaoDeItem(processoCotacaoItem, 100, 120, 12, DateTime.Today.AddMonths(1), "observacoes");


            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoMaterialCadastroVm vm = consulta.ConsultarCotacaoDeMaterial(processo.Id, fornecedor.Codigo);
            Assert.IsNotNull(vm);
            Assert.AreEqual(processo.Id, vm.IdProcessoCotacao);
            Assert.AreEqual(cotacao.Id, vm.IdCotacao);

            Assert.AreEqual(cotacao.CondicaoDePagamento.Codigo,  vm.CodigoCondicaoPagamento);
            Assert.AreEqual(cotacao.Incoterm.Codigo, vm.CodigoIncoterm);
            Assert.AreEqual("Desc Incoterm", vm.DescricaoIncoterm);
            Assert.AreEqual(fornecedor.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(), vm.DataLimiteDeRetorno);

            Assert.AreEqual("Aberto", vm.Status);
        }


        [TestMethod]
        public void QuandoConsultoUmaCotacaoDoFornecedorParaUmItemQueJaFoiPreenchidaRetornaOsDadosEsperados()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            var cotacao = processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),
                                     "Desc Incoterm");
            var processoCotacaoItem = processo.Itens.First();
            var cotacaoItem = cotacao.InformarCotacaoDeItem(processoCotacaoItem, 100, 120, 12, DateTime.Today.AddMonths(1), "observacoes");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            var itemDoProcesso = (ProcessoDeCotacaoDeMaterialItem)processo.Itens.First();
            RequisicaoDeCompra requisicaoDeCompra = itemDoProcesso.RequisicaoDeCompra;

            CotacaoMaterialItemCadastroVm vm = consulta.ConsultarCotacaoDeItemDeMaterial(processo.Id, fornecedor.Codigo,requisicaoDeCompra.Numero, requisicaoDeCompra.NumeroItem);
            
            Assert.IsNotNull(vm);

            Assert.AreEqual(processo.Id, vm.IdProcessoCotacao);
            Assert.AreEqual(cotacao.Id,vm.IdCotacao);
            Assert.AreEqual(cotacaoItem.Id,vm.IdCotacaoItem);
            Assert.AreEqual(itemDoProcesso.Id,vm.IdProcessoCotacaoItem);

            Assert.AreEqual(itemDoProcesso.Produto.Descricao, vm.Material);
            Assert.AreEqual(itemDoProcesso.Quantidade, vm.Quantidade);
            Assert.AreEqual(itemDoProcesso.UnidadeDeMedida.Descricao, vm.UnidadeDeMedida);
            Assert.AreEqual(100, vm.ValorComImpostos);
            Assert.AreEqual(100, vm.Custo);
            Assert.AreEqual(100, vm.Preco);
            Assert.AreEqual(120, vm.Mva);
        }


        [TestMethod]
        public void QuandoConsultaUmaCotacaoQueParticipaMaisDeUmFornecedorRetornaOsDadoDoFornecedorInformado()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            Fornecedor fornecedor2= DefaultObjects.ObtemFornecedorPadrao();
            processo.AdicionarFornecedor(fornecedor1);
            processo.AdicionarFornecedor(fornecedor2);
            processo.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            var processoDeCotacaoItem = processo.Itens.First();
            var cotacao1 = processo.InformarCotacao(fornecedor1.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),"Desc Incoterm");

            cotacao1.InformarCotacaoDeItem(processoDeCotacaoItem, 100, 120, 12, DateTime.Today.AddMonths(1), "observacoes");

            var cotacao2 = processo.InformarCotacao(fornecedor2.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),"Desc Incoterm");

            cotacao2.InformarCotacaoDeItem(processoDeCotacaoItem, 120, 130, 14, DateTime.Today.AddMonths(1),"observacoes");

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            var cotacao = (CotacaoMaterial) processo.FornecedoresParticipantes.Single(x => x.Fornecedor.Codigo == fornecedor1.Codigo).Cotacao;

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoMaterialCadastroVm vm = consulta.ConsultarCotacaoDeMaterial(processo.Id, fornecedor1.Codigo);

            Assert.IsNotNull(vm);
            Assert.AreEqual(processo.Id, vm.IdProcessoCotacao);
            Assert.AreEqual(cotacao.Id, vm.IdCotacao);

            Assert.AreEqual(cotacao.CondicaoDePagamento.Codigo, vm.CodigoCondicaoPagamento);
            Assert.AreEqual(cotacao.Incoterm.Codigo, vm.CodigoIncoterm);
            Assert.AreEqual("Desc Incoterm", vm.DescricaoIncoterm);
            Assert.AreEqual(fornecedor1.Codigo, vm.CodigoFornecedor);
            Assert.IsTrue(processo.DataLimiteDeRetorno.HasValue);
            Assert.AreEqual(processo.DataLimiteDeRetorno.Value.ToShortDateString(), vm.DataLimiteDeRetorno);
            Assert.AreEqual("Aberto", vm.Status);
            
        }

        [TestMethod]
        public void QuandoInformoImpostosDeUmaCotacaoRetornaOsDadosDosImpostos()
        {
            ProcessoDeCotacaoDeMaterial processo = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Fornecedor fornecedor = processo.FornecedoresParticipantes.First().Fornecedor;
            var cotacao = processo.InformarCotacao(fornecedor.Codigo, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                     DefaultObjects.ObtemIncotermPadrao(),"Desc Incoterm");

            var itemDoProcesso = (ProcessoDeCotacaoDeMaterialItem) processo.Itens.First();

            RequisicaoDeCompra requisicaoDeCompra = itemDoProcesso.RequisicaoDeCompra;

            var cotacaoItem = cotacao.InformarCotacaoDeItem(itemDoProcesso, 100, 120, 12, DateTime.Today.AddMonths(1), "observacoes");

            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Icms, 1);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.IcmsSubstituicao, 11);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.Ipi, 21);
            cotacaoItem.InformarImposto(Enumeradores.TipoDeImposto.PisCofins, 3);

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeMaterial(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaCotacaoDoFornecedor>();
            CotacaoImpostosVm vm = consulta.ConsultarCotacaoDeItemDeMaterial(processo.Id, fornecedor.Codigo, requisicaoDeCompra.Numero, requisicaoDeCompra.NumeroItem).Impostos;

            Assert.AreEqual(1, vm.IcmsAliquota);
            Assert.AreEqual(1, vm.IcmsValor);
            Assert.AreEqual(11, vm.IcmsStAliquota);
            Assert.AreEqual(11, vm.IcmsStValor);
            Assert.AreEqual(21, vm.IpiAliquota);
            Assert.AreEqual(21, vm.IpiValor);
            Assert.AreEqual(3, vm.PisCofinsValor);
            Assert.AreEqual(3, vm.PisCofinsAliquota);
        }
    }
}
