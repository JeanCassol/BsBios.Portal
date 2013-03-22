using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// classe utilizada para salvar as quotas
    /// </summary>
    public class QuotaSalvarVm
    {
        public DateTime Data { get; set; }
        public string CodigoTerminal { get; set; }
        public int CodigoMaterial { get; set; }
        public string CodigoFornecedor { get; set; }
        public decimal Peso { get; set; }
    }
}
