using BsBios.Portal.Common;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Pipeline;

namespace BsBios.Portal.IoC
{
    public class InfraServiceRegistry : Registry
    {
        public InfraServiceRegistry()
        {
            For<IValidadorUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ValidadorUsuario>();
            For<IAuthenticationProvider>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AuthenticationProvider>();
            For<IAccountService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AccountService>();
            For<IProvedorDeCriptografia>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ProvedorDeCriptografiaMd5>();
            For<IEmailService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<EmailService>();
            For<IGeradorDeSenha>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeSenha>();

            For<IGeradorDeMensagemDeEmail>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeMensagemDeEmail>();

            For<IGeradorDeEmail>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeEmail>();

            //For<IEmailServiceFactory>()
            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
            //    .Use<EmailServiceLogisticaFactory>()
            //    .Ctor<IEmailServiceFactory>("emailServiceLogisticaFactory");

            //For<IEmailServiceFactory>()
            //    .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
            //    .Use<EmailServiceSuprimentosFactory>()
            //    .Ctor<IEmailServiceFactory>("emailServiceSuprimentosFactory");


            For<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete>()
                .Named(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete);

            For<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterial>()
                .Named(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterial);

            For<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<GeradorDeEmailDeFechamentoDeProcessoDeCotacao>();

            For<IComunicacaoSap>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ComunicacaoAberturaProcessoCotacaoFrete>()
                .Named(Constantes.ComunicacaoAberturaProcessoCotacaoFrete);

            For<IComunicacaoSap>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ComunicacaoAberturaProcessoCotacaoMaterial>()
                .Named(Constantes.ComunicacaoAberturaProcessoCotacaoMaterial);

            For<IComunicacaoSap>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ComunicacaoFechamentoProcessoCotacaoFrete>()
                .Named(Constantes.ComunicacaoFechamentoProcessoCotacaoFrete);

            For<IComunicacaoSap>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<ComunicacaoFechamentoProcessoCotacaoMaterial>()
                .Named(Constantes.ComunicacaoFechamentoProcessoCotacaoMaterial);

            For<IAtualizadorDeIteracaoDoUsuario>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<AtualizadorDeIteracaoDoUsuario>();

            For<IFileService>()
                .LifecycleIs(Lifecycles.GetLifecycle(InstanceScope.PerRequest))
                .Use<FileService>();


        }
    }
}