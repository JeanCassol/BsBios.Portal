using System;
using System.Collections.Generic;

namespace BsBios.Portal.Common.Exceptions
{
    public class AgendamentosSimultaneosParaMesmaPlacaException: Exception
    {
        private readonly IList<string> _placas;

        public AgendamentosSimultaneosParaMesmaPlacaException(IList<string> placas)
        {
            _placas = placas;
            
        }

        public override string Message
        {
            get { return "Já existe agendamentos não realizados para as placas " + string.Join(", ",_placas) ; }
        }
    }
}