using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class QuantidadeLiberadaSuperouQuantidadeAdquiridaException: Exception
    {
        private readonly decimal _quantidadeLiberada;
        private readonly decimal _quantidadeAdquirida;
        public QuantidadeLiberadaSuperouQuantidadeAdquiridaException(decimal quantidadeLiberada, decimal quantidadeAdquirida)
        {
            _quantidadeLiberada = quantidadeLiberada;
            _quantidadeAdquirida = quantidadeAdquirida;
        }

        public override string Message
        {
            get { return "Não é permitido alterar a quantidade liberada para " + _quantidadeLiberada + 
                         ", pois é superior à quantidade adquirida no processo de cotação (" + _quantidadeAdquirida + ")." ; }
        }
    }
}