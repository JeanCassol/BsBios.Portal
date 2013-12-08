using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{

    public class ProcessoDeCotacaoDeFreteCabecalhoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        [DisplayName("Requisitos: ")]
        public string Requisitos { get; set; }

        [DisplayName("Endereço: ")]
        public string EnderecoDoFornecedor { get; set; }

        [DisplayName("Endereço: ")]
        public string EnderecoDoDeposito { get; set; }
        
    }
}