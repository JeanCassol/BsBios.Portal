using System.Collections.Generic;
using System.Linq;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Domain.ValueObjects;
using BsBios.Portal.Tests.DataProvider;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados.Infra.Repositories
{
    [TestClass]
    public class OrdensDeTransporteTests:RepositoryTest
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
        public void ConsigoPersistirEConsultarUmaOrdemDeTransporte()
        {
            UnitOfWorkNh.BeginTransaction();

            List<Municipio> municipios = EntidadesPersistidas.ObterDoisMunicipiosCadastrados();

            ProcessoDeCotacaoDeFrete processoDeCotacao = DefaultObjects.ObtemProcessoDeCotacaoDeFreteComCotacaoSelecionada(municipios.First(), municipios.Last());
            FornecedorParticipante fornecedorParticipante = processoDeCotacao.FornecedoresParticipantes.First();
            var condicoesDeFechamento = new List<CondicaoDoFechamentoNoSap>
            {
                new CondicaoDoFechamentoNoSap
                {
                    CodigoDoFornecedor = fornecedorParticipante.Fornecedor.Codigo,
                    NumeroGeradoNoSap = "00001"
                }
            };

            OrdemDeTransporte ordemDeTransporte = processoDeCotacao.FecharProcesso(condicoesDeFechamento).First();

            var ordensDeTransporte = ObjectFactory.GetInstance<IOrdensDeTransporte>();

            DefaultPersistedObjects.PersistirProcessoDeCotacaoDeFrete(processoDeCotacao);
            ordensDeTransporte.Save(ordemDeTransporte);

            UnitOfWorkNh.Commit();

            OrdemDeTransporte ordemDeTransporteConsultada = ordensDeTransporte.BuscaPorId(ordemDeTransporte.Id).Single();

            Assert.AreEqual(ordemDeTransporte.Id, ordemDeTransporteConsultada.Id);
            Assert.AreSame(ordemDeTransporte.Fornecedor, ordemDeTransporteConsultada.Fornecedor);
            Assert.AreEqual(ordemDeTransporte.QuantidadeAdquirida, ordemDeTransporteConsultada.QuantidadeAdquirida);
            Assert.AreEqual(ordemDeTransporte.QuantidadeLiberada, ordemDeTransporteConsultada.QuantidadeLiberada);

        }

        [TestMethod]
        public void RealizarConsultaDeOrdemDeTransportePorNotaFiscalDeColeta()
        {
            var ordensDeTransporte = ObjectFactory.GetInstance<IOrdensDeTransporte>();
            IList<OrdemDeTransporte> ordensDeTransporteEncontradas = ordensDeTransporte.ContendoNotaFiscalDeColeta("1", "1").List();

            Assert.IsFalse(ordensDeTransporteEncontradas.Any());
        }

        //[TestMethod]
        //public void ConsigoFazerJuncaoDeQueryable()
        //{
        //    var ordensDeTransporte = ObjectFactory.GetInstance<IOrdensDeTransporte>();
        //    var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

        //    //fornecedores.NomeContendo("COTRIJAL");
        //    ordensDeTransporte.BuscaPorId(10);

        //    var queryfornecedor = ordensDeTransporte.GetQuery()
        //        .Join(fornecedores.GetQuery().AsEnumerable().Where(x => x.Nome.Contains("MAURO")), ot => ot.Fornecedor, f => f,
        //            (ot, fornec) => new FornecedorFiltroVm { Codigo = ot.Fornecedor.Codigo, Nome = fornec.Nome });

        //    //var fornec = (from ot in ordensDeTransporte.GetQuery()
        //    //    join f in fornecedores.GetQuery()
        //    //        on ot.Fornecedor equals fornecedor
        //    //    select new FornecedorFiltroVm
        //    //    {
        //    //        Codigo = f.Codigo,
        //    //        Nome = f.Nome
        //    //    });

        //    //var fornecedores = ObjectFactory.GetInstance<IFornecedores>();
        //    //var produtos = ObjectFactory.GetInstance<IProdutos>();

        //    //fornecedores.NomeContendo("paulo");
        //    ////produtos.DescricaoContendo("tomate");

        //    //var fornecedor = fornecedores.GetQuery()/*.Where(x => x.Nome == "mauro")*/.Join(produtos.GetQuery(), f => f.Codigo, p => p.Codigo,
        //    //    (f, p) => f).SingleOrDefault();


        //    //var fornecedor = (from ot in ordensDeTransporte.GetQuery()
        //    //    join f in fornecedores.GetQuery()
        //    //        on ot.Fornecedor.Codigo equals f.Codigo
        //    //    select new FornecedorFiltroVm
        //    //    {
        //    //        Codigo = f.Codigo,
        //    //        Nome = f.Nome
        //    //    }).SingleOrDefault();

        //    var fornecedor = queryfornecedor.SingleOrDefault();

        //    Assert.IsNull(fornecedor);
        //}

        //[TestMethod]
        //public void ConsigoUsarNhQueryable()
        //{
        //    var uow = ObjectFactory.GetInstance<IUnitOfWorkNh>();

        //    ISessionImplementor sessionImplementation = uow.Session.GetSessionImplementation();
        //    //a tabela da direita do join não pode conter where. Acredito que seja porque o parâmetro esperado é um IEnumerable,
        //    //que faz os filtros em memória
        //    //var qf = new NhQueryable<Fornecedor>(sessionImplementation).Where(x => x.Nome == "mauro").AsQueryable();

        //    var qf = new NhQueryable<Fornecedor>(sessionImplementation);

        //    var qot = new NhQueryable<OrdemDeTransporte>(sessionImplementation).Where(x => x.Id == 10);

        //    //var fornecedor = qot.Join(qf, ot => ot.Fornecedor, f => f, (ot, f) => f).SingleOrDefault();

        //    var qr = (from ot in qot
        //        join f in qf on ot.Fornecedor equals f
        //        select new { Ordem = ot, Fornecedor = f });

        //    //se eu colocar o where depois de fazer a junção funciona

        //    var fornecedor = qr.Where(x => x.Fornecedor.Nome == "mauro").Select(x => x.Fornecedor).FirstOrDefault();


        //    Assert.IsNull(fornecedor);
        //}

        //[TestMethod]
        //public void ConsigoUtilizarExpressionDoIQueryableNoQueryOver()
        //{
        //    var fornecedores = ObjectFactory.GetInstance<IFornecedores>();

        //    fornecedores.NomeContendo("mauro");

        //    var uow = ObjectFactory.GetInstance<IUnitOfWorkNh>();

        //    var queryOver = uow.Session.QueryOver<Fornecedor>();

        //    var expression = fornecedores.GetQuery().Expression;
        //    queryOver.Where((Expression<Func<Fornecedor,bool>>) expression);

        //    var fornecedor = queryOver.Select(x => x).SingleOrDefault<Fornecedor>();

        //    Assert.IsNull(fornecedor);

        //}

    }
}
