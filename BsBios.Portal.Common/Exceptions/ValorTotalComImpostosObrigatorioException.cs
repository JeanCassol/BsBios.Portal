using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Common.Exceptions
{
    public class ValorTotalComImpostosObrigatorioException: Exception
    {
        public override string Message
        {
            get
            { 
                return "Para informar um imposto de uma Cotação é necessário preencher o campo \"Valor Total Com Impostos\".";
            }
        }
    }
}
