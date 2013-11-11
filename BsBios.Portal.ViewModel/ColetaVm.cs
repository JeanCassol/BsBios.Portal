using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ColetaVm: ColetaListagemVm
    {

        [DisplayName("Valor do Frete")]
        public decimal ValorDoFrete { get; set; }

        public NotaFiscalDeColetaVm NotaFiscal { get; set; }
        //public bool PermiteEditar { get; set; }
        public bool PermiteRealizar { get; set; }
        
        [DisplayName("Unidade de Medida")]
        public string UnidadeDeMedida { get; set; }

        [DisplayName("Preço Unitário")]
        public decimal PrecoUnitario { get; set; }
    }

    public class NotaFiscalDeColetaVm
    {
        [DisplayName("Nº do Conhecimento")]
        [Required(ErrorMessage = "Nº do Conhecimento é obrigatório")]
        public string Numero { get; set; }
        
        [DisplayName("Série")]
        [Required(ErrorMessage = "Série é obrigatório")]
        public string Serie { get; set; }
        [DisplayName("Data de Emissão")]
        [Required(ErrorMessage = "Data de Emissão é obrigatório")]
        public string DataDeEmissao { get; set; }
        [Required(ErrorMessage = "Peso é obrigatório")]
        public decimal Peso { get; set; }

        [Required(ErrorMessage = "Valor é obrigatório")]
        public decimal Valor{ get; set; }
    }
}
