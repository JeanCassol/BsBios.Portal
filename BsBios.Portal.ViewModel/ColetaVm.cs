using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ColetaVm: ColetaListagemVm
    {

        //[DisplayName("Peso Total")]
        //public decimal PesoTotal { get; set; }

        [DisplayName("Valor do Frete")]
        public decimal ValorDoFrete { get; set; }

        public NotaFiscalDeColetaVm NotaFiscal { get; set; }
        //public bool PermiteEditar { get; set; }
        public bool PermiteRealizar { get; set; }
    }

    public class NotaFiscalDeColetaVm
    {
        [DisplayName("Nº do Conhecimento")]
        [Required(ErrorMessage = "Nº do Conhecimento é obrigatório")]
        public string NumeroDoConhecimento { get; set; }
        
        [DisplayName("Série")]
        [Required(ErrorMessage = "Série é obrigatório")]
        public string Serie { get; set; }
        [DisplayName("Data de Emissão")]
        [Required(ErrorMessage = "Data de Emissão é obrigatório")]
        public string DataDeEmissao { get; set; }
        [Required(ErrorMessage = "Peso é obrigatório")]
        public decimal Peso { get; set; }

        public decimal Valor{ get; set; }
    }
}
