using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class OrdemDeTransporteCadastroVm
    {
        
        [DisplayName("Nº da Ordem de Transporte: ")]
        public int Id { get; set; }
        [DisplayName("Fornecedor: ")]
        public string NomeDoFornecedor { get; set; }
        [DisplayName("Endereço: ")]
        public string EnderecoDoFornecedor { get; set; }
        [DisplayName("Transportadora: ")]
        public string Transportadora { get; set; }
        [DisplayName("Nº do Contrato: ")]
        public string NumeroDoContrato { get; set; }

        [DisplayName("Município de Origem: ")]
        public string MunicipioDeOrigem { get; set; }
        [DisplayName("Município de Destino: ")]
        public string MunicipioDeDestino { get; set; }

        [DisplayName("Data de Validade Inicial: ")]
        public string DataDeValidadeInicial { get; set; }
        [DisplayName("Data de Validade Final: ")]
        public string DataDeValidadeFinal { get; set; }

        [DisplayName("Classificação: ")]
        public string Classificacao { get; set; }

        [DisplayName("Cadência: ")]
        public decimal Cadencia { get; set; }
        [DisplayName("Requisitos: ")]
        public string Requisitos { get; set; }

        [DisplayName("Material: ")]
        public string Material { get; set; }

        [DisplayName("Unidade de Medida: ")]
        public string UnidadeDeMedida { get; set; }

        [DisplayName("Quantidade Liberada: ")]
        public decimal QuantidadeLiberada { get; set; }
        [DisplayName("Quantidade Coletada: ")]
        public decimal QuantidadeColetada { get; set; }
        public bool PermiteAlterar { get; set; }
        public bool PermiteAdicionarColeta { get; set; }


    }
}
