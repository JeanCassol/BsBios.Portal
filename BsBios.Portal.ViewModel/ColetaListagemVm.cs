using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ColetaListagemVm: ListagemVm
    {
        public int IdDaOrdemDeTransporte { get; set; }
        public int? IdColeta { get; set; }
        [Required(ErrorMessage = "Placa é obrigatório")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "Motorista é obrigatório")]
        public string Motorista { get; set; }
        [Required(ErrorMessage = "Peso é obrigatório")]
        public decimal Peso { get; set; }

        [DisplayName("Data da Coleta")]
        [Required(ErrorMessage = "Data da Coleta é obrigatório")]
        public string DataDaColeta { get; set; }
        [DisplayName("Previsão de Chegada")]
        [Required(ErrorMessage = "Previsão de Chegada é obrigatório")]
        public string DataDePrevisaoDeChegada { get; set; }

        public string Realizado { get; set; }

        

    }



}
