using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CadastroIvaOperacaoTests
    {
        [TestMethod]
        public void QuandoCriarNovoIvaDeveRetornarMesmasPropriedades()
        {
            var ivaCadastroVm = new IvaCadastroVm()
                {
                    Codigo = "01",
                    Descricao = "IVA 01",
                };

            var cadastroIvaOperacao = new CadastroIvaOperacao();
            var iva = cadastroIvaOperacao.Criar(ivaCadastroVm);
            Assert.AreEqual("01", iva.Codigo);
            Assert.AreEqual("IVA 01", iva.Descricao);
        }

        [TestMethod]
        public void QuandoAtualizarIvaDeveAtualizarAsPropriedades()
        {
            var iva = new Iva("01", "IVA 01");

            var ivaCadastroVm = new IvaCadastroVm()
            {
                Codigo = "01",
                Descricao = "IVA 01 ALTERADO",
            };

            var ivaFornecedorOperacao = new CadastroIvaOperacao();
            ivaFornecedorOperacao.Alterar(iva,ivaCadastroVm);

            Assert.AreEqual("01", iva.Codigo);
            Assert.AreEqual("IVA 01 ALTERADO", iva.Descricao);

        }
    }
}
