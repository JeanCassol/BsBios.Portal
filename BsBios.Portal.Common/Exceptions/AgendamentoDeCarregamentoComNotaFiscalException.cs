using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class AgendamentoDeCarregamentoComNotaFiscalException: Exception
    {
        public override string Message
        {
            get { return "Não é possível adicionar notas para agendamentos de carregamento."; }
        }
    }
}
