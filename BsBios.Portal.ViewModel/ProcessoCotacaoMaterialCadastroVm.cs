﻿using System.ComponentModel.DataAnnotations;

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

        //[Required(ErrorMessage = "Material é obrigatório")]
        //[Display(Name = "Material")]
        public string CodigoMaterial { get; set; }
        //public string DescricaoMaterial { get; set; }

        //[Display(Name = "Quantidade")]
        //[Required(ErrorMessage = "Quantidade do Material é obrigatória")]
        //public int QuantidadeMaterial { get; set; }

        //[Required(ErrorMessage = "Data de Início do Leilão é obrigatório")]
        //[Display(Name = "Início do Leilão")]
        //[DataType(DataType.Date)]
        //public string DataInicioLeilao { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data Limite de Retorno")]
        [Required(ErrorMessage = "Data Limite de Retorno é obrigatória")]
        public string DataLimiteRetorno { get; set; }

        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Data Inicial de Validade da Cotação é obrigatório")]
        //[Display(Name = "Data Inicial de Validade" )]
        //public string DataValidadeCotacaoInicial { get; set; }

        //[DataType(DataType.Date)]
        //[Required(ErrorMessage = "Data Final de Validade da Cotação é obrigatório")]
        //[Display(Name = "Data Final de Validade")]
        //public string DataValidadeCotacaoFinal { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Requisitos")]
        [Required(ErrorMessage = "Requisitos é obrigatório")]
        public string Requisitos { get; set; }

        //[DataType(DataType.MultilineText)]
        //[Display(Name = "Observações")]
        //public string Observacoes { get; set; }

        public RequisicaoDeCompraVm RequisicaoDeCompraVm { get; set; }

        public bool PermiteAlterarFornecedores { get; set; }
        public bool PermiteSelecionarCotacoes { get; set; }
        public bool PermitirAbrirProcesso { get; set; }
        public bool PermiteFecharProcesso { get; set; }
        public bool PermiteSalvar { get; set; }
    }
}
