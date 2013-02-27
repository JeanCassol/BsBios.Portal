using System;
using System.Linq;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DefaultProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests.Domain.Entities
{
    [TestClass]
    public class ProcessoDeCotacaoTests
    {
        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoAsPropriedadesFicamCorretas()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, produto, 100);
            
            Assert.AreEqual("REQ0001", processoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual("00001", processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual("PROD0001", processoDeCotacao.Produto.Codigo);
            Assert.AreEqual(100, processoDeCotacao.Quantidade);

            Assert.AreEqual(0, processoDeCotacao.Fornecedores.Count);
            Assert.IsNull(processoDeCotacao.DataLimiteDeRetorno);

        }

        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoIniciaNoEstadoNaoIniciado()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            Produto produto = DefaultObjects.ObtemProdutoPadrao();
            var processoDeCotacao = new ProcessoDeCotacaoDeMaterial(requisicaoDeCompra, produto, 100);
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processoDeCotacao.Status);            
        }
        [TestMethod]
        public void QuandoAtualizoDadosComplementaresDeUmProcessoDeCotacaoAsPropriedadesSaoAlteradas()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            Assert.AreEqual(DateTime.Today.AddDays(10), processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
        }

        [TestMethod]
        public void QuandoAdicionarUmFornecedorNoProcessoEsteFicaVinculadoAoProcesso()
        {
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);

            Assert.IsNotNull(processoDeCotacaoDeMaterial.Fornecedores.SingleOrDefault(x => x.Codigo == "FORNEC0001"));

        }

        [TestMethod]
        public void QuandoAdicionaMaisDeUmaVezOMesmoFornecedorEsteApareceNaListaApenasUmaUnicaVez()
        {
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            Assert.AreEqual(1, processoDeCotacaoDeMaterial.Fornecedores.Count);
            
        }

        public void QuandoRemovoUmFornecedorEsteNaoFicaMaisVinculadoAoProcesso()
        {
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");
            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornec0002@empresa.com.br");
            var fornecedor3 = new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornec0003@empresa.com.br");

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor3);

            Assert.AreEqual(3, processoDeCotacaoDeMaterial.Fornecedores.Count);

            processoDeCotacaoDeMaterial.RemoverFornecedor("FORNEC0002");
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemDataLimiteRetornoException))]
        public void NaoEPossivelAbrirOProcessoDeCotacaoSeADataLimiteDeRetornoNaoEstiverPreenchida()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Abrir();
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemFornecedoresException))]
        public void NaoEPossivelAbrirOProcessoDeCotacaoSeNaoHouverFornecedoresVinculados()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.Abrir();
        }

        [TestMethod]
        public void QuandoAbroOProcessoDeCotacaoOStatusPassaParaAberto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.Abrir();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto, processoDeCotacaoDeMaterial.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAdicionarFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.Abrir();

            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornec0002@empresa.com.br");
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelRemoverFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");
            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornec0002@empresa.com.br");
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.Abrir();

            processoDeCotacaoDeMaterial.RemoverFornecedor("FORNEC0001");

        }

        [TestMethod]
        public void SeTentarInformarUmaCotacaoParaUmProcessoQueNaoEstejaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.AtualizarCotacao("FORNEC0001", 100, 50, new Incoterm("001", "INCOTERM 001"),
                                                         "Descrição do Incoterm");
            Assert.Fail();
        }

        [TestMethod]
        public void SeTentarInformarUmaCotacaoAposADataLimiteDeRetornoDeveGerarExcecao()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoAbrirOProcessoDeCotacaoDeveCriarUmaCotacaoParaCadaUmDosFornecedoresEscolhidosParaParticiparDoProcesso()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoSelecionarUmFornecedorDeveSerInformadoIvaCondicaoPagamentoEQuantidadeComprada()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoFecharUmProcessoDeCotacaoDeveTerPeloMenosUmFornecedorSelecionado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            Assert.Fail();
        }

        [TestMethod]
        public void QuandoFecharUmProcessoDeCotacaoDevePassarParaStatusFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Fechar();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Fechado, processoDeCotacaoDeMaterial.Status);
            Assert.Fail();

        }

        [TestMethod]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarOsDadosComplementares()
        {
            Assert.Fail();
        }


        [TestMethod]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarFornecedores()
        {
            Assert.Fail();

        }

    }
}
