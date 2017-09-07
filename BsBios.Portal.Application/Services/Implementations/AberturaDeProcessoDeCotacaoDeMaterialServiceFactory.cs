using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AberturaDeProcessoDeCotacaoDeMaterialServiceFactory : IAberturaDeProcessoDeCotacaoServiceFactory
    {
        public IAberturaDeProcessoDeCotacaoService Construir()
        {
            //var emailService = ObjectFactory
            //    .With(typeof(ContaDeEmail), ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDeSuprimentos))
            //    .GetInstance<IEmailService>();

            //var geradorDeEmailDeProcessoDeAberturaDeCotacao = ObjectFactory
            //    .With(typeof(IEmailService), emailService)
            //    .GetInstance<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterial);

            IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory geradorDeEmailFactory = new GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterialFactory();
            IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmail = geradorDeEmailFactory.Construir();

            var comunicacaoSap = ObjectFactory.GetNamedInstance<IProcessoDeCotacaoComunicacaoSap>(Constantes.ComunicacaoAberturaProcessoCotacaoMaterial);

            return ObjectFactory
                .With(typeof(IGeradorDeEmailDeAberturaDeProcessoDeCotacao), geradorDeEmail)
                .With(typeof(IProcessoDeCotacaoComunicacaoSap), comunicacaoSap)
                .GetInstance<IAberturaDeProcessoDeCotacaoService>(/*Constantes.AberturaDeProcessoDeCotacaoDeMaterial*/);
        }
    }
}