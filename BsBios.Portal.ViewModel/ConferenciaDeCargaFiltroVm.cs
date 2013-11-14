using System.ComponentModel;
using System.Text;
using BsBios.Portal.Common;

namespace BsBios.Portal.ViewModel
{
    public class ConferenciaDeCargaFiltroVm
    {
        [DisplayName("Terminal")]
        public string CodigoTerminal { get; set; }
        [DisplayName("Data de Agendamento / Previsão de Chegada")]
        public string DataAgendamento { get; set; }
        [DisplayName("Placa")]
        public string Placa { get; set; }
        [DisplayName("Número NF")]
        public string NumeroNf { get; set; }

        [DisplayName("Nome do Fornecedor")]
        public string NomeDoFornecedor { get; set; }
        [DisplayName("Realização")]
        public Enumeradores.RealizacaoDeAgendamento? RealizacaoDeAgendamento { get; set; }

        public string CodigoDeposito { get; set; }
    }
}
