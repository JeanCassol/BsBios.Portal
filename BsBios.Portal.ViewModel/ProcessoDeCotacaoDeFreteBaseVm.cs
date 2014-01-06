using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class ProcessoDeCotacaoDeFreteBaseVm
    {

        //[DisplayName("Data Limite de Retorno: ")]
        //public string DataLimiteDeRetorno { get; set; }
        [DisplayName("Material: ")]
        public virtual string Material { get; set; }
        //[DisplayName("Quantidade: ")]
        //public decimal Quantidade { get; set; }
        [DisplayName("Unidade de Medida: ")]
        public virtual string UnidadeDeMedida { get; set; }
        [DisplayName("Data de Validade Inicial: ")]
        public virtual string DataDeValidadeInicial { get; set; }
        [DisplayName("Data de Validade Final: ")]
        public virtual string DataDeValidadeFinal { get; set; }
        [DisplayName("Itinerário: ")]
        public virtual string Itinerario { get; set; }

        [DisplayName("Fornecedor da Mercadoria: ")]
        public virtual string NomeDoFornecedorDaMercadoria { get; set; }

        public virtual string CnpjDoFornecedorDaMercadoria { get; set; }

        [DisplayName("Depósito: ")]
        public virtual string NomeDoDeposito { get; set; }
        //[DisplayName("Nº do Contrato: ")]
        //public string NumeroDoContrato { get; set; }

        [DisplayName("Município de Origem: ")]
        public virtual string MunicipioDeOrigem { get; set; }
        [DisplayName("Município de Destino: ")]
        public virtual string MunicipioDeDestino { get; set; }
        [DisplayName("Classificação: ")]
        public virtual string Classificacao { get; set; }

        [DisplayName("Cadência: ")]
        public virtual decimal Cadencia { get; set; }

    }
}
