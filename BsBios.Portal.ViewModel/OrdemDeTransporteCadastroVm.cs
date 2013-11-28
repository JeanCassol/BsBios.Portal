using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class OrdemDeTransporteCadastroVm
    {
        
        [DisplayName("Nº da Ordem de Transporte: ")]
        public int Id { get; set; }
        [DisplayName("Transportadora: ")]
        public string Transportadora { get; set; }


        [DisplayName("Quantidade Liberada: ")]
        [Required(ErrorMessage = "Quantidade Liberada é obrigatório")]
        public decimal QuantidadeLiberada { get; set; }
        [DisplayName("Tolerância: ")]
        [Required(ErrorMessage = "Tolerância é obrigatório")]
        public decimal QuantidadeDeTolerancia { get; set; }

        [DisplayName("Quantidade Coletada: ")]
        public decimal QuantidadeColetada { get; set; }
        [DisplayName("Quantidade Realizada: ")]
        public decimal QuantidadeRealizada { get; set; }

        public decimal PrecoUnitario { get; set; }

        public bool PermiteAlterar { get; set; }
        public bool PermiteAdicionarColeta { get; set; }

        public ProcessoDeCotacaoDeFreteCabecalhoVm Cabecalho { get; set; }
    }
}
