using BsBios.Portal.Common;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Infra.Services.Implementations
{
    public class GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterialFactory : IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory
    {
        public IGeradorDeEmailDeAberturaDeProcessoDeCotacao Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail), ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDeSuprimentos))
                .GetInstance<IEmailService>();

            return ObjectFactory
                .With(typeof(IEmailService), emailService)
                .GetInstance<IGeradorDeEmailDeAberturaDeProcessoDeCotacao>(Constantes.GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeMaterial);
        }
    }
}