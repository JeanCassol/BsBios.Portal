using BsBios.Portal.Application.Services.Contracts;
using BsBios.Portal.Common;
using BsBios.Portal.Domain.Repositories;
using BsBios.Portal.Infra.Email;
using BsBios.Portal.Infra.Services.Contracts;
using BsBios.Portal.Infra.Services.Implementations;
using StructureMap;

namespace BsBios.Portal.Application.Services.Implementations
{
    public class FechamentoDeProcessoDeCotacaoDeFreteServiceFactory : IFechamentoDeProcessoDeCotacaoServiceFactory
    {
        public IFechamentoDeProcessoDeCotacaoDeFreteService Construir()
        {
            var emailService = ObjectFactory
                .With(typeof(ContaDeEmail),
                      ObjectFactory.GetNamedInstance<ContaDeEmail>(Constantes.ContaDeEmailDaLogistica))
                .GetInstance<IEmailService>();

            //var geradorDeEmail = ObjectFactory
            //    .With(typeof(IEmailService), emailService)
            //    .GetInstance<IGeradorDeEmailDeFechamentoDeProcessoDeCotacao>();
            var geradorDeEmail = new GeradorDeEmailDeFechamentoDeProcessoDeCotacaoDeFrete(ObjectFactory.GetInstance<IGeradorDeMensagemDeEmail>(),emailService);

            var comunicacaoSap =
                ObjectFactory.GetNamedInstance<IProcessoDeCotacaoComunicacaoSap>(Constantes.ComunicacaoFechamentoProcessoCotacaoFrete);

            //return ObjectFactory
            //    .With(typeof(IGeradorDeEmailDeFechamentoDeProcessoDeCotacao), geradorDeEmail)
            //    .With(typeof(IComunicacaoSap), comunicacaoSap)
            //    .GetInstance<IFechamentoDeProcessoDeCotacaoService>(/*Constantes.FechamentoDeProcessoDeCotacao*/);

            return new FechamentoDeProcessoDeCotacaoDeFreteService(ObjectFactory.GetInstance<IUnitOfWork>(), 
                ObjectFactory.GetInstance<IProcessosDeCotacao>(),geradorDeEmail, comunicacaoSap, 
                ObjectFactory.GetInstance<IOrdensDeTransporte>());

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