using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{

    public class ProcessoDeCotacaoDeFreteCabecalhoVm : ProcessoDeCotacaoDeFreteBaseVm
    {

        [DisplayName("Data Limite de Retorno: ")]
        public string DataLimiteDeRetorno { get; set; }
        [DisplayName("Nº do Contrato: ")]
        public string NumeroDoContrato { get; set; }

        [DisplayName("Quantidade: ")]
        public decimal Quantidade { get; set; }

        [DisplayName("Requisitos: ")]
        public string Requisitos { get; set; }

        [DisplayName("Endereço: ")]
        public string EnderecoDoFornecedor { get; set; }

        [DisplayName("Endereço: ")]
        public string EnderecoDoDeposito { get; set; }
        
    }
}