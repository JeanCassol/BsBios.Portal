using System.Collections.Generic;
using BsBios.Portal.Application.Queries.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Application.Queries
{
    [TestClass]
    public class ConsultaRelatorioDeProcessoDeCotacaoDeFreteTests
    {
        [TestMethod]
        public void ConsigoRealizarAConsultaDoRelatorio()
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
    }
}
