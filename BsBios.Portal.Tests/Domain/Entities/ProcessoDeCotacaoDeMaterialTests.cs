using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
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
            ProcessoDeCotacaoDeMaterial processoDeCotacao = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial() ;
            
            Assert.AreEqual(requisicaoDeCompra.Numero, processoDeCotacao.RequisicaoDeCompra.Numero);
            Assert.AreEqual(requisicaoDeCompra.NumeroItem, processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual(requisicaoDeCompra.Material.Codigo, processoDeCotacao.Produto.Codigo);
            Assert.AreEqual(requisicaoDeCompra.Quantidade, processoDeCotacao.Quantidade);

            Assert.AreEqual(0, processoDeCotacao.FornecedoresParticipantes.Count);
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
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            Assert.AreEqual(DateTime.Today.AddDays(10), processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
        }

        [TestMethod]
        public void QuandoAdicionarUmFornecedorNoProcessoEsteFicaVinculadoAoProcesso()
        {
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);

            Assert.IsNotNull(processoDeCotacaoDeMaterial.FornecedoresParticipantes.SingleOrDefault(x => x.Fornecedor.Codigo == fornecedor1.Codigo));

        }

        [TestMethod]
        public void QuandoAdicionaMaisDeUmaVezOMesmoFornecedorEsteApareceNaListaApenasUmaUnicaVez()
        {
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            Assert.AreEqual(1, processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count);
            
        }

        public void QuandoRemovoUmFornecedorEsteNaoFicaMaisVinculadoAoProcesso()
        {
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor3 = DefaultObjects.ObtemFornecedorPadrao();

            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor3);

            Assert.AreEqual(3, processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count);

            processoDeCotacaoDeMaterial.RemoverFornecedor(fornecedor2.Codigo);

            Assert.AreEqual(2, processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemDataLimiteRetornoException))]
        public void NaoEPossivelAbrirOProcessoDeCotacaoSeADataLimiteDeRetornoNaoEstiverPreenchida()
        {
            RequisicaoDeCompra requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();
            processoDeCotacaoDeMaterial.Abrir();
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemFornecedoresException))]
        public void NaoEPossivelAbrirOProcessoDeCotacaoSeNaoHouverFornecedoresVinculados()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.Abrir();
        }

        [TestMethod]
        public void QuandoAbroOProcessoDeCotacaoOStatusPassaParaAberto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto, processoDeCotacaoDeMaterial.Status);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAdicionarFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.Abrir();

            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelRemoverFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.Abrir();

            processoDeCotacaoDeMaterial.RemoverFornecedor(fornecedor1.Codigo);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoAtualizacaoCotacaoException))]
        public void SeTentarInformarUmaCotacaoParaUmProcessoQueNaoEstejaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001") ,"inc",100, null ,null);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoDataLimiteExpiradaException))]
        public void SeTentarInformarUmaCotacaoAposADataLimiteDeRetornoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(-1));
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir();
            processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001"), "inc", 100, null, null);
        }

        //[TestMethod]
        //public void QuandoAbrirOProcessoDeCotacaoDeveCriarUmaCotacaoParaCadaUmDosFornecedoresEscolhidosParaParticiparDoProcesso()
        //{
        //    ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
        //    processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10));
        //    var fornecedor1 = new Fornecedor("FORNEC0001", "FORNECEDOR 0001", "fornec0001@empresa.com.br");
        //    var fornecedor2 = new Fornecedor("FORNEC0002", "FORNECEDOR 0002", "fornec0002@empresa.com.br");
        //    processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
        //    processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
        //    processoDeCotacaoDeMaterial.Abrir();
        //    Assert.AreEqual(2, processoDeCotacaoDeMaterial.FornecedoresParticipantes.Count(x => x.Cotacao != null));
        //    Assert.IsNotNull(processoDeCotacaoDeMaterial.FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == "FORNEC0001").Cotacao);
        //    Assert.IsNotNull(processoDeCotacaoDeMaterial.FornecedoresParticipantes.First(x => x.Fornecedor.Codigo == "FORNEC0002").Cotacao);
        //}

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
            string codigoDoFornecedor = processoDeCotacaoDeMaterial.FornecedoresParticipantes.First().Fornecedor.Codigo;
            Cotacao cotacao = processoDeCotacaoDeMaterial.InformarCotacao(codigoDoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc", 120, null, null);
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            processoDeCotacaoDeMaterial.SelecionarCotacao(cotacao.Id, 100,iva);
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
            processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoSelecaoCotacaoException))]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelSelecionarUmFornecedor()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            int idCotacao = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Id;
            processoDeCotacao.SelecionarCotacao(idCotacao, 100, iva);
            
        }

        [TestMethod]
        public void QuandoAtualizarUmaCotacaoDoProcessoAsPropriedadesSaoAtualizadas()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2", new decimal(150.20), 180, 10);

            Cotacao cotacao = processoDeCotacao.FornecedoresParticipantes.First().Cotacao;
            Assert.IsNotNull(cotacao);
            Assert.AreSame(condicaoDePagamento,  cotacao.CondicaoDePagamento);
            Assert.AreEqual(new decimal(150.20), cotacao.ValorLiquido);
            Assert.AreEqual(180, cotacao.ValorComImpostos);
            Assert.AreEqual(10, cotacao.Mva);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm 2", cotacao.DescricaoIncoterm);

        }

        [TestMethod]
        public void QuandoSelecionaUmFornecedorACotacaoFicaMarcadaComoSelecionadaQuantidadeAdquiridaEIvaSaoPreenchidos()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Cotacao cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2", new decimal(150.20), 180, 10);
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            processoDeCotacao.SelecionarCotacao(cotacao.Id, new decimal(120.00), iva);

            Assert.IsTrue(cotacao.Selecionada);
            Assert.AreEqual(new decimal(120.00), cotacao.QuantidadeAdquirida);
            Assert.AreEqual(iva.Codigo, cotacao.Iva.Codigo);

        }

        [TestMethod]
        public void QuandoRemoverASelecaoDeUmaCotacaoFicaDesmarcadaESemQuantidade()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(10));
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir();
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            Cotacao cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2", new decimal(150.20), 180, 10);
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            processoDeCotacao.SelecionarCotacao(cotacao.Id, new decimal(120.00), iva);

            Iva ivaAlteracao = DefaultObjects.ObtemIvaPadrao();
            processoDeCotacao.RemoverSelecaoDaCotacao(cotacao.Id, ivaAlteracao);

            Cotacao cotacaoRemovida = processoDeCotacao.FornecedoresParticipantes.First().Cotacao;
            Assert.IsFalse(cotacaoRemovida.Selecionada);
            Assert.IsNull(cotacaoRemovida.QuantidadeAdquirida);
            Assert.AreSame(ivaAlteracao, cotacaoRemovida.Iva);
            
        }

    }
}
