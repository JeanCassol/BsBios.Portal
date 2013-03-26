using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    public class ConferenciaDeCargaPesquisaResultadoVm:ListagemVm
    {
        public int IdQuota { get; set; }
        public int IdAgendamento { get; set; }
        public string DataAgendamento { get; set; }
        public string DescricaoMaterial { get; set; }
        public string DescricaoFluxo { get; set; }
        public string  Placa { get; set; }
        public string NumeroNf { get; set; }
        public string CnpjEmitente { get; set; }
    }
}
