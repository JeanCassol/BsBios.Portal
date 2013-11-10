using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class QuantidadeLiberadaAbaixoDaQuantidadeColetadaException : Exception
    {
        private readonly decimal _novaQuantidadeLiberada;
        private readonly decimal _quantidadeColetada;

        public QuantidadeLiberadaAbaixoDaQuantidadeColetadaException(decimal novaQuantidadeLiberada, decimal quantidadeColetada)
        {
            _novaQuantidadeLiberada = novaQuantidadeLiberada;
            _quantidadeColetada = quantidadeColetada;
            
        }

        public override string Message
        {
            get
            {
                return "Não é permitido alterar a quantidade liberada para " + _novaQuantidadeLiberada +
                       ", pois está abaixo da quantidade total coletada na Ordem de Transporte, que é " + _quantidadeColetada + ".";
            }
        }
    }
}