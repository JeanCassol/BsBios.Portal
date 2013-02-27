using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class ProcessoDeCotacaoDataLimiteExpiradaException: Exception
    {
        private readonly DateTime _dataLimiteRetorno;
        public ProcessoDeCotacaoDataLimiteExpiradaException(DateTime dataLimiteRetorno)
        {
            _dataLimiteRetorno = dataLimiteRetorno;
        }

        public override string Message
        {
            get
            {
                return "A data limite para informar a cotação é " + _dataLimiteRetorno.ToShortDateString() + ". A Cotação não pode mais ser atualizada." ; 
            }
        }
    }
}
