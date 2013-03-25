namespace BsBios.Portal.ViewModel
{
    public class AgendamentoDeCargaListarVm: ListagemVm
    {
        public int IdAgendamento { get; set; }
        public int IdQuota { get; set; }
        public string Placa { get; set; }
        public decimal Peso { get; set; }
        public string Realizado { get; set; }
    }
}
