﻿using System.Collections.Generic;
using System.Configuration;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.DataAccess;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.IoC;
using BsBios.Portal.UI.App_Start;
using BsBios.Portal.UI.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace BsBios.Portal.TestsComBancoDeDados
{
    [TestClass]
    public class BaseTestClass
    {
        [AssemblyInitialize]
        public static void TesteInitialize(TestContext context)
        {
            SessionManager.ConfigureDataAccess(ConfigurationManager.ConnectionStrings["BsBiosTesteUnitario"].ConnectionString);
            IoCWorker.Configure();

            RemoveQueries.RemoverProcessosDeCotacaoCadastrados();
            RemoveQueries.RemoverRequisicoesDeCompraCadastradas();
            RemoveQueries.RemoverQuotasCadastradas();
            RemoveQueries.RemoverFornecedoresCadastrados();
            RemoveQueries.RemoverProdutosCadastrados();
            RemoveQueries.RemoverUsuariosCadastrados();
            RemoveQueries.RemoverCondicoesDePagamentoCadastradas();
            RemoveQueries.RemoverIvasCadastrados();
            RemoveQueries.RemoverIncotermsCadastrados();
            RemoveQueries.RemoverItinerariosCadastrados();
            RemoveQueries.RemoverUnidadesDeMedidaCadastradas();
            RemoveQueries.RemoverProcessoCotacaoIteracaoUsuarioCadastradas();

            RestaurarUsuarioConectado();

            var emailDoPortal = ConfigurationManager.GetSection("emailDoPortal") as EmailDoPortal;

            ObjectFactory.Configure(x =>
                {
                    x.AddRegistry<ControllerRegistry>();
                    if (emailDoPortal != null)
                    {
                        x.For<ContaDeEmail>()
                         .Singleton()
                         .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteLogistica + ">", emailDoPortal.Dominio,
                                               emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                               emailDoPortal.Porta)).Named(Constantes.ContaDeEmailDaLogistica);

                        x.For<ContaDeEmail>()
                         .Singleton()
                         .Use(new ContaDeEmail("Portal De Cotações <" + emailDoPortal.RemetenteSuprimentos + ">", emailDoPortal.Dominio,
                                               emailDoPortal.Usuario, emailDoPortal.Senha, emailDoPortal.Servidor,
                                               emailDoPortal.Porta)).Named(Constantes.ContaDeEmailDeSuprimentos);
                    }
                });
        }

        public static void RestaurarUsuarioConectado()
        {
            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(new UsuarioConectado("teste", "Usuário de Teste", new List<Enumeradores.Perfil> { Enumeradores.Perfil.CompradorSuprimentos })));
        }

        public static void SubstituirUsuarioConectado(UsuarioConectado usuarioConectado)
        {
            ObjectFactory.Configure(x => x.For<UsuarioConectado>()
                .HybridHttpOrThreadLocalScoped()
                .Use(() => usuarioConectado));
        }
    }
}
