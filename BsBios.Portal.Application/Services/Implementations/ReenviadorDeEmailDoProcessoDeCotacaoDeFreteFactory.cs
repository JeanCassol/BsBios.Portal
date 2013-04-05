using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class ReenviadorDeEmailDoProcessoDeCotacaoDeFreteFactory : IReenviadorDeEmailDoProcessoDeCotacaoFactory
    {
        public IReenviadorDeEmailDoProcessoDeCotacao Construir()
        {
            IGeradorDeEmailDeAberturaDeProcessoDeCotacaoFactory geradorDeEmailFactory = new GeradorDeEmailDeAberturaDeProcessoDeCotacaoDeFreteFactory();
            IGeradorDeEmailDeAberturaDeProcessoDeCotacao geradorDeEmail = geradorDeEmailFactory.Construir();

            return ObjectFactory
                .With(typeof (IGeradorDeEmailDeAberturaDeProcessoDeCotacao), geradorDeEmail)
                .GetInstance<IReenviadorDeEmailDoProcessoDeCotacao>();

        }
    }
}