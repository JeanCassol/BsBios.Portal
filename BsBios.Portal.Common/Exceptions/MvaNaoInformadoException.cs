using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class MvaNaoInformadoException: Exception
    {
        public override string Message
        {
            get { return "Quando a Cotação possui ICMS de Substituição Tributária é necessário informar o MVA."; }
        }
    }
}
