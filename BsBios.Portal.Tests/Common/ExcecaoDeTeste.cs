using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Tests.Common
{
    public class ExcecaoDeTeste: Exception
    {
        public ExcecaoDeTeste(string message) : base(message)
        {
        }
    }
}
