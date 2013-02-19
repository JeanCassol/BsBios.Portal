using System;
using BsBios.Portal.Domain.Model;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CadastroFornecedorOperacaoTests
    {
        [TestMethod]
        public void QuandoCriarNovoFornecedorDeveRetornarMesmasPropriedades()
        {
            var fornecedorCadastroVm = new FornecedorCadastroVm()
                {
                    CodigoSap = "FORNEC0001",
                    Nome = "FORNECEDOR 0001",
                    Email = "fornecedor@empresa.com.br"
                };

            var cadastroFornecedorOperacao = new CadastroFornecedorOperacao();
            var fornecedor = cadastroFornecedorOperacao.Criar(fornecedorCadastroVm);
            Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
            Assert.AreEqual("FORNECEDOR 0001", fornecedor.Nome);
            Assert.AreEqual("fornecedor@empresa.com.br", fornecedor.Email);
        }

        [TestMethod]
        public void QuandoAtualizarFornecedorDeveAtualizarAsPropriedades()
        {
            var fornecedor = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornecedor@empresa.com.br");

            var fornecedorCadastroVm = new FornecedorCadastroVm()
            {
                CodigoSap = "FORNEC0001",
                Nome = "FORNECEDOR 0001 ALTERADO",
                Email = "fornecedoralterado@empresa.com.br"
            };

            var cadastroFornecedorOperacao = new CadastroFornecedorOperacao();
            cadastroFornecedorOperacao.Atualizar(fornecedor,fornecedorCadastroVm);

            Assert.AreEqual("FORNEC0001", fornecedor.Codigo);
            Assert.AreEqual("FORNECEDOR 0001 ALTERADO", fornecedor.Nome);
            Assert.AreEqual("fornecedoralterado@empresa.com.br", fornecedor.Email);

        }
    }
}
