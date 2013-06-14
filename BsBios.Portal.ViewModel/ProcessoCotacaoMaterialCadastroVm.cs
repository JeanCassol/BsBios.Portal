using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    /// <summary>
    /// Classe utilizada na tela detalhe de cotação de materiais
    /// </summary>
    public class ProcessoCotacaoMaterialCadastroVm
    {
        public int? Id { get; set; }
        [Display(Name = "Status: ")]
        public string DescricaoStatus { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Limite de Retorno")]
        [Required(ErrorMessage = "Data Limite de Retorno é obrigatória")]
        public string DataLimiteRetorno { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [Required(ErrorMessage = "Requisitos é obrigatório")]
        public string Requisitos { get; set; }

        public bool PermiteAlterarFornecedores { get; set; }
        public bool PermiteSelecionarCotacoes { get; set; }
        public bool PermitirAbrirProcesso { get; set; }
        public bool PermiteFecharProcesso { get; set; }
        public bool PermiteSalvar { get; set; }
    }
}
