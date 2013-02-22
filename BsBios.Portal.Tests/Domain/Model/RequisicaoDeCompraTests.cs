using System;
using BsBios.Portal.Domain;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.ValueObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class RequisicaoDeCompraTests
    {
        [TestMethod]
        public void QuandoCrioUmaRequisicaoAsPropriedadesFicamCorretas()
        {
            var usuarioCriador = new Usuario("Usuario Criador", "criador", "", Enumeradores.Perfil.Comprador);
            var usuarioRequisitante = new Usuario("Usuario Requisitante", "requisitante", "", Enumeradores.Perfil.Comprador);
            var fornecedorPretendido = new Fornecedor("fpret", "Fornecedor Pretendido", null);
            var material = new Produto("MAT0001", "MATERIAL DE COMPRA", "T01");
            var dataDeRemessa = DateTime.Today.AddDays(-2);
            var dataDeLiberacao = DateTime.Today.AddDays(-1);
            var dataDeSolicitacao = DateTime.Today;

            var requisicaoDeCompra = new RequisicaoDeCompra(usuarioCriador, usuarioRequisitante,fornecedorPretendido,
                dataDeRemessa, dataDeLiberacao,dataDeSolicitacao,"CENTRO","UNT",1000,
                material, "Requisição de Compra enviada pelo SAP","ITEM001", "REQ0001");

            Assert.AreEqual("criador",requisicaoDeCompra.Criador.Login);
            Assert.AreEqual("requisitante", requisicaoDeCompra.Requisitante.Login);
            Assert.AreEqual("fpret", requisicaoDeCompra.FornecedorPretendido.Codigo);
            Assert.AreEqual("MAT0001",requisicaoDeCompra.Material.Codigo);
            Assert.AreEqual(dataDeRemessa, requisicaoDeCompra.DataDeRemessa);
            Assert.AreEqual(dataDeLiberacao, requisicaoDeCompra.DataDeLiberacao);
            Assert.AreEqual(dataDeSolicitacao, requisicaoDeCompra.DataDeSolicitacao);
            Assert.AreEqual("CENTRO",requisicaoDeCompra.Centro);
            Assert.AreEqual("UNT",requisicaoDeCompra.UnidadeMedida);
            Assert.AreEqual(1000, requisicaoDeCompra.Quantidade);
            Assert.AreEqual("Requisição de Compra enviada pelo SAP", requisicaoDeCompra.Descricao);
            Assert.AreEqual("REQ0001", requisicaoDeCompra.Numero);
            Assert.AreEqual("ITEM001", requisicaoDeCompra.NumeroItem);

        }
    }
}
