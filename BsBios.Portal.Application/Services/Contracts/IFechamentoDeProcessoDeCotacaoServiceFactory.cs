namespace BsBios.Portal.Application.Services.Contracts
{
    public interface IFechamentoDeProcessoDeCotacaoServiceFactory
    {
        IFechamentoDeProcessoDeCotacaoDeFreteService Construir();
    }

}