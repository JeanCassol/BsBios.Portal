﻿using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class RequisicaoDeCompraTests
    {
        [TestMethod]
        public void QuandoCrioUmaRequisicaoAsPropriedadesFicamCorretas()
        {
            var usuarioCriador = new Usuario("Usuario Criador", "criador", "");
            Fornecedor fornecedorPretendido = DefaultObjects.ObtemFornecedorPadrao();
            UnidadeDeMedida unidadeDeMedida = DefaultObjects.ObtemUnidadeDeMedidaPadrao();
            var material = new Produto("MAT0001", "MATERIAL DE COMPRA", "T01");
            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, "requisitante", fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao,dataDeSolicitacao,"CENTRO",unidadeDeMedida, 1000,
                material, "Requisição de Compra enviada pelo SAP","ITEM001", "REQ0001", "GC1",false);

            Assert.AreEqual("criador",requisicaoDeCompra.Criador.Login);
            Assert.AreEqual("requisitante", requisicaoDeCompra.Requisitante);
            Assert.AreEqual(fornecedorPretendido.Codigo, requisicaoDeCompra.FornecedorPretendido.Codigo);
            Assert.AreEqual("MAT0001",requisicaoDeCompra.Material.Codigo);
            Assert.AreEqual(dataDeRemessa, requisicaoDeCompra.DataDeRemessa);
            Assert.AreEqual(dataDeLiberacao, requisicaoDeCompra.DataDeLiberacao);
            Assert.AreEqual(dataDeSolicitacao, requisicaoDeCompra.DataDeSolicitacao);
            Assert.AreEqual("CENTRO",requisicaoDeCompra.Centro);
            Assert.AreSame(unidadeDeMedida,requisicaoDeCompra.UnidadeMedida);
            Assert.AreEqual(1000, requisicaoDeCompra.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoDeCompra.Descricao);
            Assert.AreEqual("REQ0001", requisicaoDeCompra.Numero);
            Assert.AreEqual("ITEM001", requisicaoDeCompra.NumeroItem);
            Assert.AreEqual("GC1", requisicaoDeCompra.CodigoGrupoDeCompra);
            Assert.IsFalse(requisicaoDeCompra.Mrp);
            Assert.AreEqual(Enumeradores.StatusRequisicaoCompra.Ativo, requisicaoDeCompra.Status);

        }

        [TestMethod]
        public void QuandoGeroUmProcessoDeCotacaoAtravesDaRequisicaoDeCompraOProcessoFicaVinculadoComARequisicao()
        {
            RequisicaoDeCompra requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            var processoDeCotacao = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();
            //Assert.AreEqual(requisicaoDeCompra.Numero, processoDeCotacao.RequisicaoDeCompra.Numero);
            //Assert.AreEqual(requisicaoDeCompra.NumeroItem, processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            var item = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.First();
            Assert.AreEqual(requisicaoDeCompra.Numero, item.RequisicaoDeCompra.Numero);
            Assert.AreEqual(requisicaoDeCompra.NumeroItem, item.RequisicaoDeCompra.NumeroItem);
        }

        [TestMethod]
        public void QuandoBloqueioRequisicaoDeCompraPassaParaEstadoBloqueado()
        {
            RequisicaoDeCompra requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            requisicaoDeCompra.Bloquear();
            Assert.AreEqual(Enumeradores.StatusRequisicaoCompra.Bloqueado, requisicaoDeCompra.Status);

        }

    }
}
