using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeOrdemDeTransporteAnaliticoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        public virtual string Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}")]
        public virtual decimal IdDaOrdemDeTransporte { get; set; }
        public virtual string Transportadora { get; set; }
        public virtual decimal QuantidadeContratada { get; set; }
        public virtual decimal QuantidadeLiberada { get; set; }
        public virtual decimal QuantidadeDeTolerancia { get; set; }
        public virtual decimal QuantidadeEmTransito { get; set; }
        public virtual decimal QuantidadeRealizada { get; set; }
        public virtual decimal QuantidadePendente { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}")]
        public virtual decimal QuantidadeDeColetasRealizadas { get; set; }
        [DisplayFormat(DataFormatString = "{0:0}")]
        public virtual decimal QuantidadeDeDiasEmAtraso { get; set; }
        public virtual decimal PercentualDeAtraso { get; set; }

    }

}
