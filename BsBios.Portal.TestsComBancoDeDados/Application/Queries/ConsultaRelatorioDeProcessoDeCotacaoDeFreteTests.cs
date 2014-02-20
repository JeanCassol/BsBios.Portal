using System;
using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DataProvider;
using BsBios.Portal.Tests.DefaultProvider;
using BsBios.Portal.TestsComBancoDeDados.Infra;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaRelatorioDeProcessoDeCotacaoDeFreteTests : RepositoryTest
    {
        [ClassInitialize]
        public static void Inicializar(TestContext testContext)
        {
            Initialize(testContext);
        }
        [ClassCleanup]
        public static void Finalizar()
        {
            Cleanup();
        }

        [TestMethod]
        public void ConsigoRealizarConsultaAnalitica()
        {
            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Classificacao = (int) Enumeradores.EscolhaSimples.Todos,
                Status = (int) Enumeradores.StatusProcessoCotacao.Aberto ,
                SelecaoDeFornecedores = (int) Enumeradores.SelecaoDeFornecedores.Selecionado,
                CodigoDoMaterial = "M_SOJA",
                CodigoDaTransportadora = "TRANSP0001",
                DataDeValidadeInicial = new DateTime(2013,6,1).ToShortDateString(),
                DataDeValidadeFinal =  new DateTime(2013,12,31).ToShortDateString(),
                DescricaoDoMaterial = "SOJA",
                NomeDoFornecedorDaMercadoria = "mauro"
            };
            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> registros = consulta.ListagemAnalitica(filtro);

            Assert.IsNotNull(registros);
        }

        [TestMethod]
        public void ConsigoFiltrarRelatorioAnaliticoPelaDataDeFechamentoIndependentementeDoHorarioQueOProcessoFoiFechado()
        {

            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();
            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteFechado(municipios.First(), municipios.Last());
            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao);

            UnitOfWorkNh.Session.Clear();

            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                SelecaoDeFornecedores = (int) Enumeradores.SelecaoDeFornecedores.Todos,
                Classificacao = (int)Enumeradores.EscolhaSimples.Todos,
                DataDeFechamento = DateTime.Now.Date.ToShortDateString()

            };
            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> registros = consulta.ListagemAnalitica(filtro);

            Assert.IsNotNull(registros);
            Assert.AreEqual(1, registros.Count);
            RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm registro = registros.First();
            Assert.AreEqual(processoDeCotacao.DataDeFechamento.ToString("G"), registro.DataDeFechamento);
        }

        [TestMethod]
        public void ConsigoRealizarConsultaSinteticaComSoma()
        {
            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Classificacao = (int) Enumeradores.EscolhaSimples.Todos,
                SelecaoDeFornecedores = (int) Enumeradores.SelecaoDeFornecedores.Todos,
                //Status = (int) Enumeradores.StatusProcessoCotacao.Aberto
            };

            IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> registros = consulta.ListagemSinteticaComSoma(filtro);

            Assert.IsNotNull(registros);

        }

        [TestMethod]
        public void ConsigoRealizarConsultaSinteticaComMedia()
        {
            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Classificacao = (int)Enumeradores.EscolhaSimples.Todos,
                SelecaoDeFornecedores = (int)Enumeradores.SelecaoDeFornecedores.Selecionado
            };

            IList<RelatorioDeProcessoDeCotacaoDeFreteSinteticoVm> registros = consulta.ListagemSinteticaComMedia(filtro);

            Assert.IsNotNull(registros);

        }

    }
}
