using System;
using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Repositories.Implementations;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Services
{
    [TestClass]
    public class CadastroDeConhecimentoDeTransporteTests
    {

        [TestInitialize]
        public void TesteInitialize()
        {
            RemoveQueries.RemoverTodosCadastros();
        }

        [TestMethod]
        public void ConsigoPersisteEConsultarUmConhecimentoDeTransporte()
        {
            var conhecimentoDeTransporteVm = GerarDadosParaConhecimentoDeTransporte();

            var conhecimentosParaCadastrar = new ListaDeConhecimentoDeTransporte() {conhecimentoDeTransporteVm};

            var cadastroDeConhecimentoDeTransporte = ObjectFactory.GetInstance<ICadastroDeConhecimentoDeTransporte>();

            cadastroDeConhecimentoDeTransporte.Salvar(conhecimentosParaCadastrar);

            var unitOfWorkNh = new UnitOfWorkNh(ObjectFactory.GetInstance<ISessionFactory>());

            //var conhecimentosDeTransporte = ObjectFactory.With(unitOfWorkNh).GetInstance<IConhecimentosDeTransporte>();
            var conhecimentosDeTransporte = new ConhecimentosDeTransporte(unitOfWorkNh);

            Console.WriteLine(".....antes de consultar novamente.....");

            ConhecimentoDeTransporte conhecimentoDeTransporte = conhecimentosDeTransporte.ComChaveEletronica("42131025174182000157550010000000020108042108").Single();

            Assert.AreEqual(2, conhecimentoDeTransporte.NotasFiscais.Count);
        }

        private static ConhecimentoDeTransporteVm GerarDadosParaConhecimentoDeTransporte()
        {
            var conhecimentoDeTransporteVm = new ConhecimentoDeTransporteVm()
            {
                ChaveEletronica = "42131025174182000157550010000000020108042108",
                CnpjDaTransportadora = "518505925",
                CnpjDoFornecedor = "2q34342423",
                DataDeEmissao = DateTime.Today.ToShortDateString(),
                Numero = "100",
                Serie = "1",
                NumeroDoContrato = "24Q424",
                PesoTotalDaCarga = 1000,
                ValorRealDoFrete = 10000,
                NotasFiscais = new ListaDeNotaFiscalDeConhecimentoDeTransporte()
                {
                    new NotaFiscalDoConhecimentoDeTransporteVm
                    {
                        Chave = "42131084684182000157550010000000020108042108",
                        Numero = "12",
                        Serie = "1"
                    },
                    new NotaFiscalDoConhecimentoDeTransporteVm
                    {
                        Chave = "42131084682582000157550010000000020108042108",
                        Numero = "13",
                        Serie = "1"
                    }
                }
            };
            return conhecimentoDeTransporteVm;
        }



        [TestMethod]
        public void ProcessarConhecimentosDeFormaAssincrona()
        {
            var conhecimentoDeTransporteVm = GerarDadosParaConhecimentoDeTransporte();

            var conhecimentosParaCadastrar = new ListaDeConhecimentoDeTransporte() { conhecimentoDeTransporteVm };

            var cadastroDeConhecimentoDeTransporte = ObjectFactory.GetInstance<ICadastroDeConhecimentoDeTransporte>();

            cadastroDeConhecimentoDeTransporte.Salvar(conhecimentosParaCadastrar);
            
        }


    }
}
