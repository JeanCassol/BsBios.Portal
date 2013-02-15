using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    public class retorno
    {
        public string retCodigo { get; set; }
        public string retTexto { get; set; }
    }

    [DataContract(Namespace = "http://portal.bsbios.com.br/")]
    public class mt_cadMaterial_portal_ret
    {
        public retorno retorno { get; set; }
    }
}
