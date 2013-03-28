using System;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class NotaFiscalVm
    {
        [StringLength(9,ErrorMessage = "Número da Nota não pode ter mais do que 9 caracteres")]
        [Required(ErrorMessage = "Número é obrigatório")]
        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Display(Name = "Série")]
        public string Serie { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessage = "Data de Emissão é obrigatório")]
        [Display(Name = "Data de Emissão")]
        public string DataDeEmissao { get; set; }

        [Required(ErrorMessage = "Nome do Emitente é obrigatório")]
        [Display(Name = "Nome do Emitente")]
        public string NomeDoEmitente { get; set; }

        [Required(ErrorMessage = "CNPJ do Emitente é obrigatório")]
        [Display(Name = "CNPJ do Emitente")]
        public string CnpjDoEmitente { get; set; }

        [Display(Name = "Nome do Contratante")]
        public string NomeDoContratante { get; set; }

        [Display(Name = "CNPJ do Contratante")]
        public string CnpjDoContratante { get; set; }

        [Display(Name = "Número do Contrato")]
        public string NumeroDoContrato { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Valor é obrigatório")]
        [Display(Name = "Valor")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "Peso é obrigatório")]
        [Display(Name = "Peso")]
        public decimal Peso { get; set; }
    }
}
