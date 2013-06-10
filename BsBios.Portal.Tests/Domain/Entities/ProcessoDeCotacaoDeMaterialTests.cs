using System;
using System.Linq;
using BsBios.Portal.Common;
using BsBios.Portal.Common.Exceptions;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
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
            
            //Assert.AreEqual(requisicaoDeCompra.Numero, processoDeCotacao.RequisicaoDeCompra.Numero);
            //Assert.AreEqual(requisicaoDeCompra.NumeroItem, processoDeCotacao.RequisicaoDeCompra.NumeroItem);
            //Assert.AreEqual(requisicaoDeCompra.Material.Codigo, processoDeCotacao.Produto.Codigo);
            //Assert.AreEqual(requisicaoDeCompra.Quantidade, processoDeCotacao.Quantidade);

            var item = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.First();

            Assert.AreEqual(requisicaoDeCompra.Numero, item.RequisicaoDeCompra.Numero);
            Assert.AreEqual(requisicaoDeCompra.NumeroItem, item.RequisicaoDeCompra.NumeroItem);
            Assert.AreEqual(requisicaoDeCompra.Material.Codigo, item.Produto.Codigo);
            Assert.AreEqual(requisicaoDeCompra.Quantidade, item.Quantidade);


            Assert.AreEqual(0, processoDeCotacao.FornecedoresParticipantes.Count);
            Assert.IsNull(processoDeCotacao.DataLimiteDeRetorno);
            Assert.IsNull(processoDeCotacao.DataDeFechamento);
            Assert.IsNull(processoDeCotacao.TextoDeCabecalho);

        }

        [TestMethod]
        public void QuandoCrioUmProcessoDeCotacaoIniciaNoEstadoNaoIniciado()
        {
            var requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            var processoDeCotacao = requisicaoDeCompra.GerarProcessoDeCotacaoDeMaterial();
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.NaoIniciado, processoDeCotacao.Status);            
        }
        [TestMethod]
        public void QuandoAtualizoDadosComplementaresDeUmProcessoDeCotacaoAsPropriedadesSaoAlteradas()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10),"requisitos alterados");
            Assert.AreEqual(DateTime.Today.AddDays(10), processoDeCotacaoDeMaterial.DataLimiteDeRetorno);
            Assert.AreEqual("requisitos alterados", processoDeCotacaoDeMaterial.Requisitos);
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
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemFornecedoresException))]
        public void NaoEPossivelAbrirOProcessoDeCotacaoSeNaoHouverFornecedoresVinculados()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
        }

        [TestMethod]
        public void QuandoAbroOProcessoDeCotacaoOStatusPassaParaAberto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto, processoDeCotacaoDeMaterial.Status);
        }

        [TestMethod]
        public void QuandoAbroProcessoDeCotacaoRegistroOComprador()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            Usuario comprador = DefaultObjects.ObtemUsuarioPadrao();
            processoDeCotacaoDeMaterial.Abrir(comprador);
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Aberto, processoDeCotacaoDeMaterial.Status);
            Assert.AreEqual(comprador, processoDeCotacaoDeMaterial.Comprador);
        }

        //23/05/2013 - Esta regra foi removida
        //[TestMethod]
        //[ExpectedException(typeof(ProcessoDeCotacaoAtualizacaoDadosException))]
        //public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAtualizarOsDadosComplementares()
        //{
        //    ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
        //    processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
        //    processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
        //    processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(10), "Requisitos alterados");
        //}

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelAdicionarFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoIniciadoAtualizacaoFornecedorException))]
        public void AposOProcessoDeCotacaoSerAbertoNaoEPossivelRemoverFornecedores()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            var fornecedor1 = DefaultObjects.ObtemFornecedorPadrao();
            var fornecedor2 = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor1);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(fornecedor2);
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            processoDeCotacaoDeMaterial.RemoverFornecedor(fornecedor1.Codigo);

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoAtualizacaoCotacaoException))]
        public void SeTentarInformarUmaCotacaoParaUmProcessoQueNaoEstejaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001"), "inc");
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFechadoAtualizacaoCotacaoException))]
        public void SeTentarInformarUmItemDaCotacaoParaUmProcessoQueNaoEstejaAbertoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001"), "inc");
            var idCotacao = processoDeCotacaoDeMaterial.FornecedoresParticipantes.First().Cotacao.Id;
            var idProcessoCotacaoItem = processoDeCotacaoDeMaterial.Itens.First().Id;
            processoDeCotacaoDeMaterial.InformarCotacaoDeItem(idProcessoCotacaoItem, idCotacao, 10, null, 5,DateTime.Today.AddDays(4), "obs");
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoDataLimiteExpiradaException))]
        public void SeTentarInformarUmaCotacaoAposADataLimiteDeRetornoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(-1), processoDeCotacaoDeMaterial.Requisitos);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001"), "inc");
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoDataLimiteExpiradaException))]
        public void SeTentarInformarUmItemDaCotacaoAposADataLimiteDeRetornoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processoDeCotacaoDeMaterial.Atualizar(DateTime.Today.AddDays(-1), processoDeCotacaoDeMaterial.Requisitos);
            processoDeCotacaoDeMaterial.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacaoDeMaterial.Abrir(DefaultObjects.ObtemUsuarioPadrao());

            var cotacao = processoDeCotacaoDeMaterial.InformarCotacao("FORNEC0001", DefaultObjects.ObtemCondicaoDePagamentoPadrao(), new Incoterm("001", "INCOTERM 001"), "inc");
            var processoDeCotacaoItem = processoDeCotacaoDeMaterial.Itens.First();
            cotacao.InformarCotacaoDeItem(processoDeCotacaoItem, 100, null, 100, DateTime.Today.AddMonths(1),"obs fornec");

        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoFecharSemCotacaoSelecionadaException))]
        public void QuandoFecharUmProcessoDeCotacaoENaoHouverPeloMenosFornecedorSelecionadoDeveGerarExcecao()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();
            processoDeCotacaoDeMaterial.Fechar("texto de cabeçalho", "nota de cabeçalho");
        }

        [TestMethod]
        public void QuandoFecharUmProcessoDeCotacaoDevePassarParaStatusFechadoEPreencherDataDeFechamento()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacaoDeMaterial = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();
            string codigoDoFornecedor = processoDeCotacaoDeMaterial.FornecedoresParticipantes.First().Fornecedor.Codigo;
            var cotacao =  processoDeCotacaoDeMaterial.InformarCotacao(codigoDoFornecedor, DefaultObjects.ObtemCondicaoDePagamentoPadrao(),
                                                        DefaultObjects.ObtemIncotermPadrao(), "inc");
            var processoCotacaoItem = processoDeCotacaoDeMaterial.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem) cotacao.InformarCotacaoDeItem(processoCotacaoItem, 120, null, 100, DateTime.Today.AddMonths(1), "obs fornec");
                
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            cotacaoItem.Selecionar(100,iva);
            processoDeCotacaoDeMaterial.Fechar("texto de cabeçalho","nota de cabeçalho");
            Assert.AreEqual(Enumeradores.StatusProcessoCotacao.Fechado, processoDeCotacaoDeMaterial.Status);
            Assert.AreEqual(DateTime.Now.Date, processoDeCotacaoDeMaterial.DataDeFechamento);
            Assert.AreEqual("texto de cabeçalho", processoDeCotacaoDeMaterial.TextoDeCabecalho);
            Assert.AreEqual("nota de cabeçalho", processoDeCotacaoDeMaterial.NotaDeCabecalho);
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAtualizacaoDadosException))]
        public void AposOProcessoDeCotacaoSerFechadoNaoEPossivelAtualizarOsDadosComplementares()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacao.Atualizar(DateTime.Today.AddDays(20),"Requisitos alterados");
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
            var idProcessoCotacaoItem = processoDeCotacao.Itens.First().Id;
            processoDeCotacao.SelecionarCotacao(idCotacao,idProcessoCotacaoItem, 100, iva);
            
        }

        [TestMethod]
        public void QuandoAtualizarUmaCotacaoDoProcessoAsPropriedadesSaoAtualizadas()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2");

            var cotacao = (CotacaoMaterial) processoDeCotacao.FornecedoresParticipantes.First().Cotacao;
            Assert.IsNotNull(cotacao);
            Assert.AreSame(condicaoDePagamento,  cotacao.CondicaoDePagamento);
            Assert.AreSame(incoterm, cotacao.Incoterm);
            Assert.AreEqual("Descrição do Incoterm 2", cotacao.DescricaoIncoterm);

        }

        [TestMethod]
        public void QuandoAtualizoUmItemDaCotacaoDoProcessoAsPropriedadesSaoAtualizadas()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialComCotacaoDoFornecedor();
            var idCotacao = processoDeCotacao.FornecedoresParticipantes.First().Cotacao.Id;
            var idProcessoCotacaoItem = processoDeCotacao.Itens.First().Id;
            var cotacaoItem = (CotacaoMaterialItem) processoDeCotacao.InformarCotacaoDeItem(idProcessoCotacaoItem, idCotacao, 180, 10, 100, DateTime.Today.AddMonths(1), "obs fornec");

            Assert.AreEqual(new decimal(180), cotacaoItem.Preco);
            Assert.AreEqual(180, cotacaoItem.ValorComImpostos);
            Assert.AreEqual(10, cotacaoItem.Mva);
            Assert.AreEqual(100, cotacaoItem.QuantidadeDisponivel);
            Assert.AreEqual(DateTime.Today.AddMonths(1), cotacaoItem.PrazoDeEntrega);

        }

        [TestMethod]
        public void QuandoSelecionaUmFornecedorACotacaoFicaMarcadaComoSelecionadaQuantidadeAdquiridaEIvaSaoPreenchidos()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            var processoCotacaoItem = processoDeCotacao.Itens.First();
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2");
            var cotacaoItem = (CotacaoMaterialItem) cotacao.InformarCotacaoDeItem(processoCotacaoItem, new decimal(150.20), 180, 10, DateTime.Today.AddMonths(1), "obs fornec");
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            cotacaoItem.Selecionar(new decimal(120.00), iva);

            Assert.IsTrue(cotacaoItem.Selecionada);
            Assert.AreEqual(new decimal(120.00), cotacaoItem.QuantidadeAdquirida);
            Assert.AreEqual(iva.Codigo, cotacaoItem.Iva.Codigo);

        }

        [TestMethod]
        public void QuandoRemoverASelecaoDeUmaCotacaoFicaDesmarcadaESemQuantidade()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            Fornecedor fornecedor = DefaultObjects.ObtemFornecedorPadrao();
            processoDeCotacao.AdicionarFornecedor(fornecedor);
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
            Incoterm incoterm = DefaultObjects.ObtemIncotermPadrao();
            CondicaoDePagamento condicaoDePagamento = DefaultObjects.ObtemCondicaoDePagamentoPadrao();
            var cotacao = processoDeCotacao.InformarCotacao(fornecedor.Codigo, condicaoDePagamento, incoterm, "Descrição do Incoterm 2");
            var processoCotacaoItem = processoDeCotacao.Itens.First();
            var cotacaoItem = (CotacaoMaterialItem)cotacao.InformarCotacaoDeItem(processoCotacaoItem, new decimal(150.20), 180, 10, DateTime.Today.AddMonths(1), "obs fornec");
            Iva iva = DefaultObjects.ObtemIvaPadrao();
            cotacaoItem.Selecionar(new decimal(120.00), iva);

            Iva ivaAlteracao = DefaultObjects.ObtemIvaPadrao();
            cotacaoItem.RemoverSelecao(ivaAlteracao);

            Assert.IsFalse(cotacaoItem.Selecionada);
            Assert.IsNull(cotacaoItem.QuantidadeAdquirida);
            Assert.AreSame(ivaAlteracao, cotacaoItem.Iva);
            
        }

        [TestMethod]
        public void QuandoQuantidadeTotalForMenorOuIgualQueAQuantidadeDoProcessoNaoSuperouQuantidadeDoProcesso()
        {
            //retorna processo com quantidade 1000
            ProcessoDeCotacao processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            //Assert.IsFalse(processoDeCotacao.SuperouQuantidadeSolicitada(1000));
            var item = (ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.First();
            Assert.IsFalse(item.SuperouQuantidadeSolicitada(1000));
        }

        [TestMethod]
        public void QuandoQuantidadeTotalForMaiorQueAQuantidadeDoProcessoNaoSuperouQuantidadeDoProcesso()
        {
            //retorna processo com quantidade 1000
            ProcessoDeCotacao processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            //Assert.IsTrue(processoDeCotacao.SuperouQuantidadeSolicitada(1001));
            var item = (ProcessoDeCotacaoDeMaterialItem)processoDeCotacao.Itens.First();
            Assert.IsTrue(item.SuperouQuantidadeSolicitada(1001));
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoSemItemException))]
        public void NaoEPermitidoAbrirProcessoDeCotacaoSemItens()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAtualizado();
            processoDeCotacao.AdicionarFornecedor(DefaultObjects.ObtemFornecedorPadrao());
            processoDeCotacao.RemoverItem(processoDeCotacao.Itens.First());
            processoDeCotacao.Abrir(DefaultObjects.ObtemUsuarioPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAlterarItensException))]
        public void NaoEPermitidoAdicionarItensEmUmProcessoDeCotacaoAberto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();
            processoDeCotacao.AdicionarItem(DefaultObjects.ObtemRequisicaoDeCompraPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAlterarItensException))]
        public void NaoEPermitidoRemoverItensDeUmProcessoDeCotacaoAberto()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialAbertoPadrao();
            processoDeCotacao.RemoverItem(processoDeCotacao.Itens.First());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAlterarItensException))]
        public void NaoEPermitidoAdicionarItensDeUmProcessoDeCotacaoFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacao.AdicionarItem(DefaultObjects.ObtemRequisicaoDeCompraPadrao());
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessoDeCotacaoAlterarItensException))]
        public void NaoEPermitidoRemoverItensDeUmProcessoDeCotacaoFechado()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialFechado();
            processoDeCotacao.RemoverItem(processoDeCotacao.Itens.First());
        }

        [TestMethod]
        public void QuandoAdicionarItemEmUmProcessoDeCotacaoARequisicaoDeCompraFicaVinculadaAoItem()
        {
            RequisicaoDeCompra requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            Assert.IsFalse(requisicaoDeCompra.GerouProcessoDeCotacao);
            processoDeCotacao.AdicionarItem(requisicaoDeCompra);
            Assert.IsTrue(requisicaoDeCompra.GerouProcessoDeCotacao);
        }

        [TestMethod]
        public void QuandoRemovoItemDeUmProcessoDeCotacaoOVinculoEntreARequisicaoEoItemERemovido()
        {
            ProcessoDeCotacaoDeMaterial processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            RequisicaoDeCompra requisicaoDeCompra = ((ProcessoDeCotacaoDeMaterialItem) processoDeCotacao.Itens.First()).RequisicaoDeCompra;
            Assert.IsTrue(requisicaoDeCompra.GerouProcessoDeCotacao);
            processoDeCotacao.RemoverItem(processoDeCotacao.Itens.First());
            Assert.IsFalse(requisicaoDeCompra.GerouProcessoDeCotacao);
        }


        [TestMethod]
        [ExpectedException(typeof(RequisicaoDeCompraAssociadaAOutroProcessoDeCotacaoException))]
        public void NaoEPermitidoUtilizarMesmoItemDeRequisicaoEmMaisDeUmProcessoDeCotacao()
        {
            RequisicaoDeCompra requisicaoDeCompra = DefaultObjects.ObtemRequisicaoDeCompraPadrao();
            ProcessoDeCotacaoDeMaterial processo1 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processo1.AdicionarItem(requisicaoDeCompra);

            //tenta adicionar a mesma requisição de compra no segundo processo
            ProcessoDeCotacaoDeMaterial processo2 = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
            processo2.AdicionarItem(requisicaoDeCompra);

        }


    }
}
