using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Infra.Model
{
    [TestClass]
    public class ProcessoCotacaoIteracaoUsuarioTests
    {
        [TestMethod]
        public void QuandoCrioUmaIteracaoDoUsuarioParaProcessoDeCotacaoIniciaComoNaoVisualizadoPeloFornecedor()
        {
            var iteracaoUsuario = new ProcessoCotacaoIteracaoUsuario(1);
            Assert.AreEqual(1, iteracaoUsuario.IdFornecedorParticipante);
            Assert.IsFalse(iteracaoUsuario.VisualizadoPeloFornecedor);
        }

        [TestMethod]
        public void QuandoInformoQueFornecedorVisualizouProcesssoDeCotacaoPassaParaVisualizado()
        {
            var iteracaoUsuario = new ProcessoCotacaoIteracaoUsuario(1);
            iteracaoUsuario.FornecedorVisualizou();
            Assert.IsTrue(iteracaoUsuario.VisualizadoPeloFornecedor);
        }
    
    }
}
