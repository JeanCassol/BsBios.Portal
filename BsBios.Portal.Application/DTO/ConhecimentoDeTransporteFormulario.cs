﻿using System.ComponentModel;

namespace BsBios.Portal.Application.DTO
{
    public class ConhecimentoDeTransporteFormulario: ConhecimentoDeTransporteListagem
    {
        [DisplayName("Série")]
        public string Serie { get; set; }

        public bool PermiteAtribuir { get; set; }
    }
}
