using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaRelatorioDeProcessoDeCotacaoDeFreteTests : RepositoryTest
    {
        [TestMethod]
        public void ConsigoRealizarConsultaAnalitica()
        {
            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Classificacao = (int) Enumeradores.EscolhaSimples.Todos,
                //SelecaoDeFornecedores = (int) Enumeradores.SelecaoDeFornecedores.NaoSelecionado,
                CodigoDaTransportadora = "TRANSP0001"
            };
            IList<RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm> registros = consulta.ListagemAnalitica(filtro);

            Assert.IsNotNull(registros);
        }

        [TestMethod]
        public void ConsigoRealizarConsultaSinteticaComSoma()
        {
            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeProcessoDeCotacaoDeFrete>();
            var filtro = new RelatorioDeProcessoDeCotacaoDeFreteFiltroVm
            {
                Classificacao = (int) Enumeradores.EscolhaSimples.Todos,
                SelecaoDeFornecedores = (int) Enumeradores.SelecaoDeFornecedores.Selecionado
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
