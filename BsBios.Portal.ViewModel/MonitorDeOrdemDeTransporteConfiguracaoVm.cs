using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BsBios.Portal.ViewModel
{
    public class MonitorDeOrdemDeTransporteConfiguracaoVm
    {
        /// <summary>
        /// Nome das propriedades que devem ser agrupadas, separadas por virgulas
        /// </summary>
        public string Agrupamentos { get; set; }

        [DisplayName("Código do Material")]
        public string CodigoDoMaterial { get; set; }
        [DisplayName("Descrição do Material")]
        public string DescricaoDoMaterial { get; set; }
        [DisplayName("Código do Fornecedor da Mercadoria")]
        public string CodigoDoFornecedorDaMercadoria { get; set; }
        [DisplayName("Nome do Fornecedor da Mercadoria")]
        public string NomeDoFornecedorDaMercadoria { get; set; }
        [DisplayName("Código da Transportadora")]
        public string CodigoDaTransportadora { get; set; }
        [DisplayName("Nome da Transportadora")]
        public string NomeDaTransportadora { get; set; }
        [DisplayName("Município de Origem")]
        public string CodigoDoMunicipioDeOrigem { get; set; }
        [DisplayName("Município de Destino")]
        public string CodigoDoMunicipioDeDestino { get; set; }
        
    }

    public class MonitorOrdemDeTransporteParametroVm : MonitorDeOrdemDeTransporteConfiguracaoVm
    {
        [Required(ErrorMessage = "Intervalo de Atualização é obrigatório")]
        [Display(Name = "Intervalo de Atualização (segundos)")]
        [Range(1,1000000,ErrorMessage = "Intervalo deve ser um valor entre 1 e 1.000.000")]
        public int InterValoDeAtualizacao { get; set; }
    }
}