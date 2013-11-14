using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoDeFreteCabecalhoVm
    {
        [DisplayName("Requisitos: ")]
        public string Requisitos { get; set; }
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
        [DisplayName("Data de Validade Inicial: ")]
        public string DataDeValidadeInicial { get; set; }
        [DisplayName("Data de Validade Final: ")]
        public string DataDeValidadeFinal { get; set; }
        [DisplayName("Itinerário: ")]
        public string Itinerario { get; set; }

        [DisplayName("Fornecedor da Mercadoria: ")]
        public string NomeDoFornecedor { get; set; }

        [DisplayName("Endereço: ")]
        public string CnpjDoFornecedor { get; set; }

        public string EnderecoDoFornecedor { get; set; }
        [DisplayName("Depósito:")]
        public string NomeDoDeposito { get; set; }
        [DisplayName("Endereço: ")]
        public string EnderecoDoDeposito { get; set; }
        [DisplayName("Nº do Contrato: ")]
        public string NumeroDoContrato { get; set; }

        [DisplayName("Município de Origem: ")]
        public string MunicipioDeOrigem { get; set; }
        [DisplayName("Município de Destino: ")]
        public string MunicipioDeDestino { get; set; }
        [DisplayName("Classificação: ")]
        public string Classificacao { get; set; }

        [DisplayName("Cadência: ")]
        public decimal Cadencia { get; set; }


        
    }
}