using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Infra.Model;
using BsBios.Portal.Infra.Repositories.Contracts;
using BsBios.Portal.Infra.Services.Contracts;
using StructureMap;
using StructureMap.Pipeline;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class FechamentoDeProcessoDeCotacaoDeFreteServiceFactory : IFechamentoDeProcessoDeCotacaoServiceFactory
    {
        public IFechamentoDeProcessoDeCotacaoService Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail),
                      ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDaLogistica))
                .GetInstance<IEmailService>();

            var geradorDeEmail = ObjectFactory
                .With(typeof(IEmailService), emailService)
                .GetInstance<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>();

            var comunicacaoSap =
                ObjectFactory.GetNamedInstance<IProcessoDeCotacaoComunicacaoSap>(Constantes.ComunicacaoFechamentoProcessoCotacaoFrete);

            return ObjectFactory
                .With(typeof(IGeradorDeEmailDeFechamentoDeProcessoDeCotacao), geradorDeEmail)
                .With(typeof(IProcessoDeCotacaoComunicacaoSap), comunicacaoSap)
                .GetInstance<IFechamentoDeProcessoDeCotacaoService>(/*Constantes.FechamentoDeProcessoDeCotacao*/);


            //return ObjectFactory
            //    .With(typeof(IUnitOfWork),ObjectFactory.GetInstance<IUnitOfWork>())
            //    .With(typeof(IProcessosDeCotacao), ObjectFactory.GetInstance<IProcessosDeCotacao>())
            //    .With(typeof(IGeradorDeEmailDeFechamentoDeProcessoDeCotacao), geradorDeEmail)
            //    .With(typeof(IComunicacaoSap), comunicacaoSap)
            //    .GetInstance<IFechamentoDeProcessoDeCotacaoService>();

            //return ObjectFactory
            //    .With("unitOfWork").EqualTo(ObjectFactory.GetInstance<IUnitOfWork>())
            //    .With("processosDeCotacao").EqualTo(ObjectFactory.GetInstance<IProcessosDeCotacao>())
            //    .With("geradorDeEmail").EqualTo(geradorDeEmail)
            //    .With("comunicacaoSap").EqualTo(comunicacaoSap)
            //    .GetInstance<IFechamentoDeProcessoDeCotacaoService>();

        }
    }

}