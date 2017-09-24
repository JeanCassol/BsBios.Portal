using System.ComponentModel;

namespace BsBios.Portal.ViewModel
{
    public class CotacaoFreteCadastroVm : CotacaoInformarVm
    {
        public ProcessoDeCotacaoDeFreteCabecalhoVm Cabecalho { get; set; }

        /// <summary>
        /// usuada para indicar se os campos da tela serão habilitados para edição
        /// </summary>
        public bool PermiteEditar { get; set; }
        public bool PermiteAlterarPreco { get; set; }
        public int IdFornecedorParticipante { get; set; }

        //[DisplayName("Requisitos: ")]
        //public string Requisitos { get; set; }
        //[DisplayName("Data Limite de Retorno: ")]
        //public string DataLimiteDeRetorno { get; set; }
        //[DisplayName("Status: ")]
        //public string Status { get; set; }
        //[DisplayName("Material: ")]
        //public string Material { get; set; }
        //[DisplayName("Quantidade: ")]
        //public decimal Quantidade { get; set; }
        //[DisplayName("Unidade de Medida: ")]
        //public string UnidadeDeMedida { get; set; }
        //[DisplayName("Data de Validade Inicial: ")]
        //public string DataDeValidadeInicial { get; set; }
        //[DisplayName("Data de Validade Final: ")]
        //public string DataDeValidadeFinal { get; set; }
        //[DisplayName("Itinerário: ")]
        //public string Itinerario { get; set; }

        

    }
}