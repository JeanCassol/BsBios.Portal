using System;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Services
{
    [TestClass]
    public class CadastroCondicaoPagamentoOperacaoTests
    {
        [TestMethod]
        public void QuandoCriarNovaCondicaoDePagamentoDeveRetornarMesmasPropriedades()
        {
            var condicaoPagamentoCadastroVm = new CondicaoDePagamentoCadastroVm()
                {
                    Codigo = "C001",
                    Descricao = "CONDICAO 001",
                };

            var cadastroCondicaoPagamentoOperacao = new CadastroCondicaoPagamentoOperacao();
            var condicaoPagamento = cadastroCondicaoPagamentoOperacao.Criar(condicaoPagamentoCadastroVm);
            Assert.AreEqual("C001", condicaoPagamento.Codigo);
            Assert.AreEqual("CONDICAO 001", condicaoPagamento.Descricao);
        }

        [TestMethod]
        public void QuandoAtualizarCondicaoDePagamentoDeveAtualizarAsPropriedades()
        {
            var condicaoPagamento = new CondicaoDePagamento("C001", "CONDICAO 0001");

            var condicaoPagamentoCadastroVm = new CondicaoDePagamentoCadastroVm()
            {
                Codigo = "C001",
                Descricao = "CONDICAO 0001 ALTERADA"
            };

            var cadastroCondicaoPagamentoOperacao = new CadastroCondicaoPagamentoOperacao();
            cadastroCondicaoPagamentoOperacao.Alterar(condicaoPagamento, condicaoPagamentoCadastroVm);

            Assert.AreEqual("C001", condicaoPagamento.Codigo);
            Assert.AreEqual("CONDICAO 0001 ALTERADA", condicaoPagamento.Descricao);
        }
    }
}
