using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.Domain
{
    public class Enumeradores
    {
        public enum Perfil
        {
            [Description("Comprador")]
            Comprador = 1,
            [Description("Fornecedor")]
            Fornecedor = 2
        }
    }
}
