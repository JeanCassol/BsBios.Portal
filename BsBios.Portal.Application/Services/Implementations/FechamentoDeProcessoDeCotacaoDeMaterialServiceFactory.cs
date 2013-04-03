using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class FechamentoDeProcessoDeCotacaoDeMaterialServiceFactory : IFechamentoDeProcessoDeCotacaoServiceFactory
    {
        public IFechamentoDeProcessoDeCotacaoService Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail),
                      ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDeSuprimentos))
                .GetInstance<IEmailService>();

            var geradorDeEmail = ObjectFactory
                .With(typeof(IEmailService), emailService)
                .GetInstance<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>();

            var comunicacaoSap =
                ObjectFactory.GetNamedInstance<IComunicacaoSap>(Constantes.ComunicacaoFechamentoProcessoCotacaoMaterial);

            return ObjectFactory
                .With(typeof(IGeradorDeEmailDeFechamentoDeProcessoDeCotacao), geradorDeEmail)
                .With(typeof(IComunicacaoSap), comunicacaoSap)
                .GetInstance<IFechamentoDeProcessoDeCotacaoService>();
        }
    }
}