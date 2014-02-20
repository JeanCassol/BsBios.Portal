using System;
using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class RelatorioDeProcessoDeCotacaoDeFreteAnaliticoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        public string Terminal { get; set; }

        [DisplayName("Status: ")]
        public virtual object Status { get; set; }

        public string DescricaoDoStatus { get; set; }

        [DisplayName("Nº Cotação")]
        public int IdDoProcessoDeCotacao { get; set; }
        [DisplayName("Nº do Contrato: ")]
        public string NumeroDoContrato { get; set; }

        [DisplayName("Data Limite de Retorno: ")]
        public string DataLimiteDeRetorno { get; set; }
        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }

        public string Transportadora { get; set; }
        public string Visualizado { get; set; }
        public string Selecionado { get; set; }
        public decimal QuantidadeDisponivel { get; set; }
        public decimal QuantidadeLiberada { get; set; }
        public decimal ValorComImpostos { get; set; }
        public DateTime? DataHoraDeFechamento { get; set; }

        public string DataDeFechamento
        {
            get
            {
                return DataHoraDeFechamento.HasValue ?  DataHoraDeFechamento.Value.ToString("G") : ""; 
            }
        }

    }
}
