using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFreteFactory : IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory
    {
        public IGeradorDeEmailDeAberturaDeProcessoDeCotacao Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail), ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDaLogistica))
                .GetInstance<IEmailService>();

            return ObjectFactory
                .With(typeof(IEmailService), emailService)
                .GetInstance<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFrete);
        }
    }
}