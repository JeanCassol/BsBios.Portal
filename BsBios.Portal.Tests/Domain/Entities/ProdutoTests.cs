using System;
using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Model
{
    [TestClass]
    public class ProdutoTests
    {
        [TestMethod]
        public void QuandoCrioUmProdutoConsigoAcessarAsPropriedades()
        {
            var produto = new Produto("SAP0001", "Produto de Teste", "01");
            Assert.AreEqual("SAP0001", produto.Codigo);
            Assert.AreEqual("Produto de Teste", produto.Descricao);
            Assert.AreEqual("01", produto.Tipo);
        }

        [TestMethod]
        public void QuandoAtualizarDescricaoDeveAcessarNovoValor()
        {
            var produto = new Produto("SAP0001", "Produto de Teste", "01");
            produto.Atualizar("Produto de Teste atualizado", "02");
            Assert.AreEqual("Produto de Teste atualizado", produto.Descricao);
            Assert.AreEqual("02", produto.Tipo);
        }

        [TestMethod]
        public void
            QuandoAdicionoUmaListaDeFornecedoresEmUmProdutoSemFornecedoresTodosOsFornecedoresSaoAdicionadosNoProduto()
        {
            var produto = new Produto("PROD0001", "Produto de Teste", "01");
            var fornecedores = new List<Fornecedor>()
                {
                    new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor01@empresa.com.br"),
                    new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor02@empresa.com.br")
                };

            produto.AdicionarFornecedores(fornecedores);

            Assert.AreEqual(2, produto.Fornecedores.Count);
        }

        [TestMethod]
        public void QuandoAdicionarUmaListaComUmFornecedorQueJaEstaVinculadoNoMaterialDeveDeAdicionarApenasOsFornecedoresNovos()
        {
            //inicialmente cria um produto com dois fornecedores
            var produto = new Produto("PROD0001", "Produto de Teste", "01");
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor01@empresa.com.br");
            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornecedor02@empresa.com.br");
            
            produto.AdicionarFornecedores(new List<Fornecedor>() {fornecedor1 , fornecedor2});

            //adiciona mais dois fornecedores ao produto, sendo que um deles já existe
            var fornecedor3 = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor03@empresa.com.br");

            produto.AdicionarFornecedores(new List<Fornecedor>() 
            { new Fornecedor("FORNEC0001", "FORNECEDOR 0001 ALTERADO", "fornecedor01@empresa.com.br"), fornecedor3 });

            Assert.AreEqual(3, produto.Fornecedores.Count);
        }
    }
}
