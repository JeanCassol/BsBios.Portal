using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CalculadorDeBaseDeCalculoPadrao : ICalculadorDeBaseDeCalculo
    {
        public decimal Calcular(CotacaoItem cotacaoItem)
        {
            return cotacaoItem.Preco;
        }
    }
}