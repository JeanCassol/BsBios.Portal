using System.Collections.Generic;

namespace BsBios.Portal.ViewModel
{
    public class FornecedorCotacaoVm
    {
        public string Codigo { get; set; }
        public decimal PrecoInicial { get; set; }
        public decimal PrecoFinal { get; set; }
        public decimal[] Precos { get; set; }
        public bool Selecionada { get; set; }
        public decimal QuantidadeAdquirida { get; set; }
    }
}
