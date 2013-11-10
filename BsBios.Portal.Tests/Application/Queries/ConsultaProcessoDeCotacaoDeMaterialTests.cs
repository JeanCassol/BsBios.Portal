using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BsBios.Portal.Application.Queries.Builders;
using BsBios.Portal.Application.Queries.Implementations;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BsBios.Portal.Tests.Application.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeMaterialTests
    {
        [TestMethod]
        public void QuandoNaoFiltraPorStatusAutomaticamenteDesconsideraProcessosDeCotacaoCancelados()
        {
            var processosDeCotacaoMock = new Mock<IProcessosDeCotacao>(MockBehavior.Strict);
            processosDeCotacaoMock.Setup(x => x.DesconsideraCancelados())
                .Returns(processosDeCotacaoMock.Object);

            processosDeCotacaoMock.Setup(x => x.FiltraPorTipo(It.IsAny<Enumeradores.TipoDeCotacao>()))
                .Returns(processosDeCotacaoMock.Object);

            processosDeCotacaoMock.Setup(x => x.CodigoDoProdutoContendo(It.IsAny<string>()))
                .Returns(processosDeCotacaoMock.Object);

            processosDeCotacaoMock.Setup(x => x.DescricaoDoProdutoContendo(It.IsAny<string>()))
                .Returns(processosDeCotacaoMock.Object);

            IQueryable<ProcessoDeCotacao> queryable = new EnumerableQuery<ProcessoDeCotacao>(new List<ProcessoDeCotacao>());

            processosDeCotacaoMock.Setup(x => x.GetQuery())
                .Returns(queryable);

            var builderMock = new Mock<IBuilder<Fornecedor, FornecedorCadastroVm>>(MockBehavior.Strict);

            var processosCotacaoIteracaoUsuarioMock = new Mock<IProcessoCotacaoIteracoesUsuario>(MockBehavior.Strict);

            var consulta = new ConsultaProcessoDeCotacaoDeMaterial(processosDeCotacaoMock.Object, builderMock.Object,processosCotacaoIteracaoUsuarioMock.Object);

            consulta.Listar(new PaginacaoVm(), new ProcessoCotacaoMaterialFiltroVm
            {
                TipoDeCotacao = 2
            });

            processosDeCotacaoMock.Verify(x => x.DesconsideraCancelados(), Times.Once());
            processosDeCotacaoMock.Verify(x => x.FiltraPorStatus(It.IsAny<Enumeradores.StatusProcessoCotacao>()), Times.Never());
        }
    }
}
