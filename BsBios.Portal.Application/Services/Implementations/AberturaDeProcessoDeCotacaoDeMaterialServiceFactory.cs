using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AberturaDeProcessoDeCotacaoDeMaterialServiceFactory : IAberturaDeProcessoDeCotacaoServiceFactory
    {
        public IAberturaDeProcessoDeCotacaoService Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail), ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDeSuprimentos))
                .GetInstance<IEmailService>();

            var geradorDeEmailDeProcessoDeAberturaDeCotacao = ObjectFactory
                .With(typeof(IEmailService), emailService)
                .GetInstance<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterial);

            var comunicacaoSap = ObjectFactory.GetNamedInstance<IComunicacaoSap>(Constantes.ComunicacaoAberturaProcessoCotacaoMaterial);

            return ObjectFactory
                .With(typeof(IGeradorDeEmailDeAberturaDeProcessoDeCotacao), geradorDeEmailDeProcessoDeAberturaDeCotacao)
                .With(typeof(IComunicacaoSap), comunicacaoSap)
                .GetInstance<IAberturaDeProcessoDeCotacaoService>(/*Constantes.AberturaDeProcessoDeCotacaoDeMaterial*/);
        }
    }
}