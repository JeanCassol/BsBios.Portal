using System;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaProcessoDeCotacaoDeFreteTests
    {
        [TestMethod]
        public void ConsigoConsultarOsDadosDeUmProcesso()
        {
            ProcessoDeCotacaoDeFrete processo = DefaultObjects.ObtemProcessoDeCotacaoDeFrete();
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processo);

            var consulta = ObjectFactory.GetInstance<IConsultaProcessoDeCotacaoDeFrete>();

            ProcessoCotacaoFreteCadastroVm viewModel = consulta.ConsultaProcesso(processo.Id);

            Assert.IsNotNull(viewModel);
        }

    }
}
