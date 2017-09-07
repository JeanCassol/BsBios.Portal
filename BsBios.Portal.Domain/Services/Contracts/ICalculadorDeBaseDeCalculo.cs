using BsBios.Portal.Domain.Entities;

namespace BsBios.Portal.Domain.Services.Contracts
{
    public interface ICalculadorDeBaseDeCalculo
    {
        decimal Calcular(CotacaoItem cotacaoItem);
    }
}