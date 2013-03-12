using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.IoC;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

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
            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(new UsuarioConectado("teste", "Usuário de Teste", new List<Enumeradores.Perfil>{Enumeradores.Perfil.CompradorSuprimentos})));
            Queries.RemoverProcessosDeCotacaoDeMateriaisCadastradas();
            Queries.RemoverRequisicoesDeCompraCadastradas();
            Queries.RemoverFornecedoresCadastrados();
            Queries.RemoverProdutosCadastrados();
            Queries.RemoverUsuariosCadastrados();
            Queries.RemoverCondicoesDePagamentoCadastradas();
            Queries.RemoverIvasCadastrados();
            Queries.RemoverIncotermsCadastrados();
            Queries.RemoverItinerariosCadastrados();
        }
    }
}
