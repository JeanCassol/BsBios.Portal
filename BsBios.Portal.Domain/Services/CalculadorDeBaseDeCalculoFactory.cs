using BsBios.Portal.Common;
using BsBios.Portal.Domain.Entities;
using BsBios.Portal.Domain.Services.Contracts;
using BsBios.Portal.Domain.Services.Implementations;

namespace BsBios.Portal.Domain.Services
{
    public class CalculadorDeBaseDeCalculoFactory
    {
        public ICalculadorDeBaseDeCalculo Construir(Enumeradores.TipoDeImposto tipoDeImposto, Produto produto)
        {
            if (tipoDeImposto == Enumeradores.TipoDeImposto.Ipi && produto.MaterialPrima)
            {
                return new CalculadorDeBaseDeCalculoComCreditoDeImpostos();
            }
            return new CalculadorDeBaseDeCalculoPadrao();
        }
    }
}