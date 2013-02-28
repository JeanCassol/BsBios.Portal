using System.Configuration;
using System.Web.Http;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BsBios.Portal.Tests
{
    [TestClass]
    public class BaseTestClass
    {
        [AssemblyInitialize]
        public static void TesteInitialize(TestContext context)
        {
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBiosTesteUnitario"].ConnectionString);
            IoCWorker.Configure();
            Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();
            Queries.RemoverRequisicoesDeCompraCadastradas();
            Queries.RemoverFornecedoresCadastrados();
            Queries.RemoverProdutosCadastrados();
            Queries.RemoverUsuariosCadastrados();
            Queries.RemoverCondicoesDePagamentoCadastradas();
            Queries.RemoverIvasCadastrados();
            Queries.RemoverIncotermsCadastrados();
        }
    }
}
