using System;

namespace BsBios.Portal.Common.Exceptions
{
    public class PesoAgendadoSuperiorAoPesoDaQuotaException : Exception
    {
        private readonly decimal _pesoAgendado;
        private readonly decimal _pesoTotal;

        public PesoAgendadoSuperiorAoPesoDaQuotaException(decimal pesoAgendado, decimal pesoDaQuota)
        {
            _pesoAgendado = pesoAgendado;
            _pesoTotal = pesoDaQuota;
        }

        public override string Message
        {
            get { return "O total de Peso Agendado ("+ _pesoAgendado +") superou a quota de " + _pesoTotal +  "."; }
        }
    }
}