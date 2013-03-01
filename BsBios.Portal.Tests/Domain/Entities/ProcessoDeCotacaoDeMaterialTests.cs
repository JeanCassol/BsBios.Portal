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
            
            Assert.AreEqual(requisicaoDeCompra.Numero, processoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual(requisicaoDeCompra.NumeroItem, processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual(requisicaoDeCompra.Material.Codigo, processoDeCotacao.Produto.Codigo);
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
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto, processoDeCotacaoDeMaterial.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
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
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoAtualizacaoCotacaoException))]
        public void SeTentarInformarUmaCotacaoParaUmProcessoQueNaoEstejaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacaoDeMaterial.AtualizarCotacao("FORNEC0001", 100,  new Incoterm("001", "INCOTERM 001"),
                                                         "Descrição do Incoterm");
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoDataLimiteExpiradaException))]
        public void SeTentarInformarUmaCotacaoAposADataLimiteDeRetornoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(-1));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.AtualizarCotacao("FORNEC0001", 100, DefaultObjects.ObtemIncotermPadrao(), "INCOTERM");
        }

        [TestMethod]
        public void QuandoAbrirOProcessoDeCotacaoDeveCriarUmaCotacaoParaCadaUmDosFornecedoresEscolhidosParaParticiparDoProcesso()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialPadrao();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");
            var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornec0002@empresa.com.br");
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.Abrir();
            Assert.AreEqual(2, processoDeCotacaoDeMaterial.Cotacoes.Count);
            Assert.IsNotNull(processoDeCotacaoDeMaterial.Cotacoes.First(x => x.Fornecedor.Codigo == "FORNEC0001"));
            Assert.IsNotNull(processoDeCotacaoDeMaterial.Cotacoes.First(x => x.Fornecedor.Codigo == "FORNEC0002"));
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFecharSemCotacaoSelecionadaException))]
        public void QuandoFecharUmProcessoDeCotacaoENaoHouverPeloMenosFornecedorSelecionadoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            processoDeCotacaoDeMaterial.Fechar();
        }

        [TestMethod]
        public void QuandoFecharUmProcessoDeCotacaoDevePassarParaStatusFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoAbertoPadrao();
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            processoDeCotacaoDeMaterial.SelecionarCotacao(processoDeCotacaoDeMaterial.Fornecedores.First().Codigo, 100,iva, condicaoDePagamento);
            processoDeCotacaoDeMaterial.Fechar();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Fechado, processoDeCotacaoDeMaterial.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(20));
        }


        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacao.AdicionarFornecedor(new Fornecedor("FORNEC0003", "FORNECEDOR 0003", "fornecedor0003@empresa.com.br"));
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoSelecaoCotacaoException))]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelSelecionarUmFornecedor()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            processoDeCotacao.SelecionarCotacao("FORNEC0001", 100, iva, condicaoDePagamento);
            
        }

    }
}
