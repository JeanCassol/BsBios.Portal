using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class AlterarOrdemDeTransporteFechadaException: Exception
    {
        public override string Message
        {
            get { return "Não é permitido alterar uma Ordem de Transporte fechada."; }
        }
    }
}
