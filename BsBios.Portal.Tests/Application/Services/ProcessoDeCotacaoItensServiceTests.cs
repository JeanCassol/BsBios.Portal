using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Services.Implementations;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Tests.Common;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Services
{
    [TestClass]
    public class ProcessoDeCotacaoItensServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IProcessosDeCotacao> _processosDeCotacaoMock;
        private readonly Mock<IRequisicoesDeCompra> _requisicoesDeCompraMock;
        private readonly ProcessoDeCotacaoDeMaterialItensService _service;
        private readonly ProcessoDeCotacaoDeMaterial _processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeMaterialNaoIniciado();
        private readonly IList<RequisicaoDeCompra> _requisicoesDeCompras;

        public ProcessoDeCotacaoItensServiceTests()
        {
            _unitOfWorkMock = CommonMocks.DefaultUnitOfWorkMock();

            _requisicoesDeCompras = new List<RequisicaoDeCompra>
            {
                ((ProcessoDeCotacaoDeMaterialItem)  _processoDeCotacao.Itens.First()).RequisicaoDeCompra,
                DefaultObjects.ObtemRequisicaoDeCompraComId(),
                DefaultObjects.ObtemRequisicaoDeCompraComId()
            };

            _processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Returns(_processosDeCotacaoMock.Object);
            _processosDeCotacaoMock.Setup(x => x.Single())
                                   .Returns(_processoDeCotacao);
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                .Callback(
                (ProcessoDeCotacao processoDeCotacao) => Assert.IsNotNull(processoDeCotacao));

            _requisicoesDeCompraMock = new Mock<IRequisicoesDeCompra>(MockBehavior.Strict);
            _requisicoesDeCompraMock.Setup(x => x.FiltraPorIds(It.IsAny<int[]>()))
                                    .Returns((int[] ids) => _requisicoesDeCompras.Where(x => ids.Contains(x.Id)).ToList());
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                                    .Callback(CommonGenericMocks<RequisicaoDeCompra>.DefaultSaveCallBack(_unitOfWorkMock));

            _service  = new ProcessoDeCotacaoDeMaterialItensService(_unitOfWorkMock.Object, _processosDeCotacaoMock.Object,_requisicoesDeCompraMock.Object);

        }

        [TestMethod]
        public void QuandoOsItensSaoAtualizadosComSucessoOcorrePersistencia()
        {
            _processosDeCotacaoMock.Setup(x => x.Save(It.IsAny<ProcessoDeCotacao>()))
                .Callback((ProcessoDeCotacao processoDeCotacao) => Assert.IsNotNull(processoDeCotacao));

            _service.AtualizarItens(1, new List<int> { 1, 2 });
            _processosDeCotacaoMock.Verify(x => x.Save(It.IsAny<ProcessoDeCotacao>()), Times.Once());
            CommonVerifications.VerificaCommitDeTransacao(_unitOfWorkMock);
        }

        [TestMethod]
        public void QuandoOcorreErroAoSalvarItensOcorreRollBack()
        {
            //força um erro em um dos métodos utilizados pelo serviço
            _processosDeCotacaoMock.Setup(x => x.BuscaPorId(It.IsAny<int>()))
                                   .Throws(new ExcecaoDeTeste("Ocorreu erro ao consultar o Processo de Cotação"));

            try
            {
                _service.AtualizarItens(1, new List<int> { 2, 3 });
                Assert.Fail("Deveria ter gerado excessão");
            }
            catch (ExcecaoDeTeste)
            {
                CommonVerifications.VerificaRollBackDeTransacao(_unitOfWorkMock);
                
            }

        }
        [TestMethod]
        public void QuandoAListaDeRequisicoesContemNovosItensOsItensSaoAdicionadosAoProcessoDeCotacao()
        {
            //a lista contém o item atual do processo e um item novo
            var ids = new List<int> {_requisicoesDeCompras.Select(x => x.Id).ElementAt(0),_requisicoesDeCompras.Select(x => x.Id).ElementAt(1)};
            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                //callback verifica que a requisição de compra está vinculada ao item do processo de cotação
                                    .Callback((RequisicaoDeCompra requisicao) => Assert.IsNotNull(requisicao.ProcessoDeCotacaoItem));
            Assert.AreEqual(1, _processoDeCotacao.Itens.Count);
            _service.AtualizarItens(_processoDeCotacao.Id,  ids);
            Assert.AreEqual(2, _processoDeCotacao.Itens.Count);
        }

        [TestMethod]
        public void QuandoAListaDeRequisicoesNaoContemAlgumItemEsteItemERemovidoDoProcessoDeCotacao()
        {
            //adiciona mais um elemento no item
            _processoDeCotacao.AdicionarItem(_requisicoesDeCompras.ElementAt(1));

            Assert.AreEqual(2, _processoDeCotacao.Itens.Count);
            //envia apenas o segundo elemento na lista. o primeiro será removido
            var ids = new List<int>
                {
                    _processoDeCotacao.Itens.Select(x => ((ProcessoDeCotacaoDeMaterialItem) x).RequisicaoDeCompra.Id)
                                      .ElementAt(1)
                };

            _requisicoesDeCompraMock.Setup(x => x.Save(It.IsAny<RequisicaoDeCompra>()))
                //callback verifica que o vincuulo entre a requisição de compra e o processo de cotação foi removido
                                    .Callback((RequisicaoDeCompra requisicao) => Assert.IsNull(requisicao.ProcessoDeCotacaoItem));

            _service.AtualizarItens(_processoDeCotacao.Id, ids);
            Assert.AreEqual(1, _processoDeCotacao.Itens.Count);
        }

    }
}
