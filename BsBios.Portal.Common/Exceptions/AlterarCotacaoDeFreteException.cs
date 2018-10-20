using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class AlterarCotacaoDeFreteException : Exception
    {
        public AlterarCotacaoDeFreteException()
            : base("Não é possível alterar os valores informados em uma Cotação de Frete.")
        {
        }
    }
}