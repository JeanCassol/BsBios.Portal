using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class AberturaDeProcessoDeCotacaoDeFreteServiceFactory : IAberturaDeProcessoDeCotacaoServiceFactory
    {
        public IAberturaDeProcessoDeCotacaoService Construir()
        {
            //var emailService = ObjectFactory
            //    .With(typeof(ContaDeEmail), ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDaLogistica))
            //    .GetInstance<IEmailService>();

            //var geradorDeEmailDeProcessoDeAberturaDeCotacao = ObjectFactory
            //    .With(typeof(IEmailService), emailService)
            //    .GetInstance<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete);

            IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory geradorDeEmailFactory = new GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFreteFactory();
            IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmail = geradorDeEmailFactory.Construir();

            var comunicacaoSap = ObjectFactory.GetNamedInstance<IProcessoDeCotacaoComunicacaoSap>(Constantes.ComunicacaoAberturaProcessoCotacaoFrete);

            return ObjectFactory
                .With(typeof(IGeradorDeEmailDeAberturaDeProcessoDeCotacao), geradorDeEmail)
                .With(typeof(IProcessoDeCotacaoComunicacaoSap), comunicacaoSap)
                .GetInstance<IAberturaDeProcessoDeCotacaoService>(/*Constantes.AberturaDeProcessoDeCotacaoDeFrete*/);

        }
    }
}