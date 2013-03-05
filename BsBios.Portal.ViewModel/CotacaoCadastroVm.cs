﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoCadastroVm: CotacaoAtualizarVm
    {
        [DisplayName("Descrição: ")]
        public string Descricao { get; set; }
        [DisplayName("Data Limite de Retorno")]
        public string DataLimiteDeRetorno { get; set; }
        [DisplayName("Status: ")]
        public string Status { get; set; }
        [DisplayName("Material: ")]
        public string Material { get; set; }
        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }
        [DisplayName("Unidade de Medida: ")]
        public string UnidadeDeMedida { get; set; }
    }
}
