using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{

    public class ProcessoDeCotacaoDeFreteCabecalhoVm : ProcessoDeCotacaoDeFreteBaseVm
    {
        [DisplayName("Nº Cotação: ")]
        public int Numero { get; set; }

        [DisplayName("Status: ")]
        public virtual string Status { get; set; }

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

        [DisplayName("Terminal: ")]
        public string Terminal { get; set; }

        [DisplayName("Resposta: ")]
        public string Resposta { get; set; }

        
    }
}