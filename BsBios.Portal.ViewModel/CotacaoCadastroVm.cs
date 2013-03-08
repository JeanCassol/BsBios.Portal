﻿using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoCadastroVm: CotacaoInformarVm
    {
        [DisplayName("Descrição: ")]
        public string DescricaoDoProcessoDeCotacao { get; set; }
        [DisplayName("Data Limite de Retorno: ")]
        public string DataLimiteDeRetorno { get; set; }
        [DisplayName("Status: ")]
        public string Status { get; set; }
        [DisplayName("Material: ")]
        public string Material { get; set; }
        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }
        [DisplayName("Unidade de Medida: ")]
        public string UnidadeDeMedida { get; set; }
        /// <summary>
        /// usuada para indicar se os campos da tela serão habilitados para edição
        /// </summary>
        public bool PermiteEditar { get; set; }
    }
}