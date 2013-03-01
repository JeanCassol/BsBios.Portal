using System;
using System.Web.Mvc;
using BsBios.Portal.UI.Controllers;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.UI.Controllers
{
    [TestClass]
    public class ProcessoDeCotacaoTests
    {
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoCorretamenteDeveRetornarMensagemDeSucesso()
        {
            var processoDeCotacaoController = new ProcessoDeCotacaoController();
            JsonResult retorno = processoDeCotacaoController.AtualizarFornecedores(new AtualizacaoDosFornecedoresDoProcessoDeCotacaoVm());
        }
        [TestMethod]
        public void QuandoAtualizarOsFornecedoresDoProcessoDeCotacaoComErroDeveRetornarMensagemDeErro()
        {
        }
    }
}
