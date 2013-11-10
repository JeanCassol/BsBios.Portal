﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ColetaListagemVm: ListagemVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Placa é obrigatório")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "Motorista é obrigatório")]
        public string Motorista { get; set; }
        [Required(ErrorMessage = "Quantidade é obrigatório")]
        public decimal Peso { get; set; }

        [DisplayName("Previsão de Chegada")]
        [Required(ErrorMessage = "Previsão de Chegada é obrigatório")]
        public string DataDePrevisaoDeChegada { get; set; }

        public bool PermiteEditar { get; set; }

    }



}