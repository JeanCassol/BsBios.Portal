using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.Services.Implementations;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Domain.Services
{

    [TestClass]
    public class ProcessadorDeColetaTests
    {

        private Mock<IOrdensDeTransporte> GerarMockParaRepositorioDeOrdemDeTransporte(ConhecimentoDeTransporte conhecimentoDeTransporte, IList<OrdemDeTransporte> ordensDeTransporte)
        {
            var mock = new Mock<IOrdensDeTransporte>(MockBehavior.Strict);

            mock.Setup(x => x.ComPeriodoDeValidadeContendoAData(conhecimentoDeTransporte.DataDeEmissao)).Returns(mock.Object);
            mock.Setup(x => x.ComColetaAberta()).Returns(mock.Object);
            mock.Setup(x => x.DoFornecedorDaMercadoria(conhecimentoDeTransporte.CnpjDoFornecedor)).Returns(mock.Object);
            mock.Setup(x => x.DaTransportadora(conhecimentoDeTransporte.CnpjDaTransportadora)).Returns(mock.Object);

            mock.Setup(x => x.IncluirProcessoDeCotacao()).Returns(mock.Object);

            mock.Setup(x => x.List()).Returns(ordensDeTransporte);

            return mock;

        }

        private Mock<IFornecedores> GerarMockParaFornecedor(ConhecimentoDeTransporte conhecimentoDeTransporte, OrdemDeTransporte ordemDeTransporte, 
            bool encontrarFornecedor, bool encontrarTransportadora)
        {
            var mockFornecedores = new Mock<IFornecedores>(MockBehavior.Strict);

            mockFornecedores.Setup(x => x.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDoFornecedor)).Returns(encontrarFornecedor ? ordemDeTransporte.ProcessoDeCotacaoDeFrete.FornecedorDaMercadoria: null);

            mockFornecedores.Setup(x => x.BuscaPeloCnpj(conhecimentoDeTransporte.CnpjDaTransportadora)).Returns(encontrarTransportadora ? ordemDeTransporte.Fornecedor: null);

            return mockFornecedores;

        }
            
        [TestMethod]
        public void QuandoNaoEncontrarFornecedorDaMercadoriaTemQueGravarStatusDeErroComMensagemAdequada()
        {
            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>(){ordemDeTransporte} );

            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte, ordemDeTransporte, false, false);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.AreEqual(Enumeradores.StatusDoConhecimentoDeTransporte.Erro, conhecimentoDeTransporte.Status);
            Assert.AreEqual("Fornecedor da Mercadoria não encontrado", conhecimentoDeTransporte.MensagemDeErroDeProcessamento);

        }

        [TestMethod]
        public void QuandoNaoEncontrarTransportadoraTemQueGravarStatusDeErroComMensagemAdequada()
        {
            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>() { ordemDeTransporte });

            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte,ordemDeTransporte, true,false);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.AreEqual(Enumeradores.StatusDoConhecimentoDeTransporte.Erro, conhecimentoDeTransporte.Status);
            Assert.AreEqual("Transportadora não encontrada", conhecimentoDeTransporte.MensagemDeErroDeProcessamento);

        }

            
        [TestMethod]
        public void QuandoEncontrarApenasUmaOrdemDeTransporteTemQuePassarStatusParaAtribuido()
        {

            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>() { ordemDeTransporte });
            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte,ordemDeTransporte, true, true);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.AreEqual(Enumeradores.StatusDoConhecimentoDeTransporte.Atribuido, conhecimentoDeTransporte.Status);

        }

        [TestMethod]
        public void QuandoEncontrarApenasUmaOrdemDeTransporteTemQueGerarColetaParaOrdemDeTransporteEPassarStatusParaAtribuido()
        {
            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>() { ordemDeTransporte });
            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte,ordemDeTransporte, true, true);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            OrdemDeTransporte ordemVinculada = processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.IsNotNull(ordemVinculada);

            Assert.AreEqual(1, ordemVinculada.Coletas.Count);

        }

        //Não sei ainda se vai ter esta regra
        [TestMethod]
        public void QuandoVinculoDoConhecimentoComOrdemAdicionarMaisColetasDoQueQuantidadeLiberadaDeveGravarMensagemDeErroDeNegocio()
        {
            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(90);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>() { ordemDeTransporte });
            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte, ordemDeTransporte, true, true);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            OrdemDeTransporte ordemVinculada = processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.IsNull(ordemVinculada);

            Assert.AreEqual(0, ordemDeTransporte.Coletas.Count);

        }

        [TestMethod]
        public void QuandoEncontrarMaisDeUmaOrdemDeTransporteTemQueGerarOrdensCandidatasENaoAtribuirConhecimentoComOrdemDeTransporte()
        {
            ConhecimentoDeTransporte conhecimentoDeTransporte = DefaultObjects.ObterConhecimentoDeTransporte();
            OrdemDeTransporte ordemDeTransporte1 = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);
            OrdemDeTransporte ordemDeTransporte2 = DefaultObjects.ObtemOrdemDeTransporteComQuantidade(1000);

            Mock<IOrdensDeTransporte> mockOrdensDeTransporte = GerarMockParaRepositorioDeOrdemDeTransporte(conhecimentoDeTransporte, new List<OrdemDeTransporte>() { ordemDeTransporte1, ordemDeTransporte2 });
            Mock<IFornecedores> mockFornecedores = GerarMockParaFornecedor(conhecimentoDeTransporte, ordemDeTransporte1, true, true);

            var processadorDeColeta = new ProcessadorDeColeta(mockOrdensDeTransporte.Object, mockFornecedores.Object);

            OrdemDeTransporte ordemVinculada = processadorDeColeta.Processar(conhecimentoDeTransporte);

            Assert.IsNull(ordemVinculada);

            Assert.AreEqual(2, conhecimentoDeTransporte.OrdensDeTransporte.Count());

            Assert.AreEqual(0, ordemDeTransporte1.Coletas.Count);
            Assert.AreEqual(0, ordemDeTransporte2.Coletas.Count);
            
        }
    }
}
