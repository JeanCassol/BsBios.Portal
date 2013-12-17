namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeOrdemDeTransporteAnaliticoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        public virtual string Id { get; set; }
        public virtual decimal IdDaOrdemDeTransporte { get; set; }
        public virtual string Transportadora { get; set; }
        public virtual decimal QuantidadeContratada { get; set; }
        public virtual decimal QuantidadeLiberada { get; set; }
        public virtual decimal QuantidadeDeTolerancia { get; set; }
        public virtual decimal QuantidadeEmTransito { get; set; }
        public virtual decimal QuantidadeRealizada { get; set; }
        public virtual decimal QuantidadePendente { get; set; }
        public virtual int QuantidadeDeColetasRealizadas { get; set; }
        public virtual int QuantidadeDeDiasEmAtraso { get; set; }
        public virtual decimal PercentualDeAtraso { get; set; }

    }

}
