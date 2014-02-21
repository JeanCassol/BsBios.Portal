using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Application.Queries.Contracts;
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
    public class ConsultaRelatorioDeOrdemDeTransporteTests : RepositoryTest
    {
        [TestMethod]
        public void ConsigoRealizarConsultaDoRelatorioAnalitico()
        {

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());

            IEnumerable<CondicaoDoFechamentoNoSap> condicoesDeFechamento = processoDeCotacao.FornecedoresSelecionados.Select(x => new CondicaoDoFechamentoNoSap
            {
                CodigoDoFornecedor = x.Fornecedor.Codigo,
                NumeroGeradoNoSap = "00001"
            });


            IList<OrdemDeTransporte> ordensDeTransporte = processoDeCotacao.FecharProcesso(condicoesDeFechamento);
            OrdemDeTransporte ordemDeTransporte = ordensDeTransporte.First();

            DefaultPersistedObjects.PersistirOrdensDeTransporte(ordensDeTransporte, processoDeCotacao);

            var consulta = ObjectFactory.GetInstance<IConsultaRelatorioDeOrdemDeTransporte>();
            var filtro = new RelatorioDeOrdemDeTransporteFiltroVm()
            {
                CodigoDaTransportadora = ordemDeTransporte.Fornecedor.Codigo
            };
            IList<RelatorioDeOrdemDeTransporteAnaliticoVm> registros = consulta.ListagemAnalitica(filtro);

            Assert.AreEqual(1, registros.Count);

            RelatorioDeOrdemDeTransporteAnaliticoVm vm = registros.First();

            Assert.AreEqual(ordemDeTransporte.Id, vm.IdDaOrdemDeTransporte);
            Assert.AreEqual(ordemDeTransporte.Fornecedor.Nome, vm.Transportadora);

        }
    }
}
