using System.Collections.Generic;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
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
                    DefaultObjects.ObtemFornecedorPadrao(),
                    DefaultObjects.ObtemFornecedorPadrao()
                };

            produto.AdicionarFornecedores(fornecedores);

            Assert.AreEqual(2, produto.Fornecedores.Count);
        }

        [TestMethod]
        public void QuandoAdicionarUmaListaComUmFornecedorQueJaEstaVinculadoNoMaterialDeveDeAdicionarApenasOsFornecedoresNovos()
        {
            //inicialmente cria um produto com dois fornecedores
            var produto = new Produto("PROD0001", "Produto de Teste", "01");
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            
            produto.AdicionarFornecedores(new List<Fornecedor>() {fornecedor1 , fornecedor2});

            //adiciona mais dois fornecedores ao produto, sendo que um deles já existe
            var fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();

            produto.AdicionarFornecedores(new List<Fornecedor>() 
            { new Fornecedor(fornecedor1.Codigo, "FORNECEDOR 0001 ALTERADO", "fornecedor01@empresa.com.br",
                fornecedor1.Cnpj, fornecedor1.Municipio, fornecedor1.Uf,false), fornecedor3 });

            Assert.AreEqual(3, produto.Fornecedores.Count);
        }
    }
}
