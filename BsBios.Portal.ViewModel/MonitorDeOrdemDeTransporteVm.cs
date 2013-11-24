namespace BsBios.Portal.ViewModel
{
    public class MonitorDeOrdemDeTransporteVm
    {
        public virtual string Id { get; set; }
        public virtual string Material { get; set; }
        public virtual string Fornecedor { get; set; }
        public virtual decimal QuantidadeLiberada { get; set; }
        public virtual decimal QuantidadeEmTransito { get; set; }
        public virtual decimal PrevisaoDeChegadaNoDia { get; set; }
        public virtual decimal QuantidadePendente { get; set; }

    }
}