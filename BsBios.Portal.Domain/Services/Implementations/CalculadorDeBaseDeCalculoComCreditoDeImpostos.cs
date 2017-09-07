using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;

namespace BsBios.Portal.Domain.Services.Implementations
{
    public class CalculadorDeBaseDeCalculoComCreditoDeImpostos : ICalculadorDeBaseDeCalculo
    {
        public decimal Calcular(CotacaoItem cotacaoItem)
        {
            return cotacaoItem.Preco - cotacaoItem.ValorDoImposto(Enumeradores.TipoDeImposto.Icms) -
                   cotacaoItem.ValorDoImposto(Enumeradores.TipoDeImposto.PisCofins);
        }
    }
}