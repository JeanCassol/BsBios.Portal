using System.ComponentModel;
using BsBios.Portal.Common;
using BsBios.Portal.ViewModel;

namespace BsBios.Portal.Application.DTO
{
    public class ConhecimentoDeTransporteListagem: ListagemVm
    {
        public string ChaveEletronica { get; set; }
        [DisplayName("Data de Emissão")]
        public string DataDeEmissao { get; set; }
        [DisplayName("Fornecedor")]
        public string CodigoDoFornecedor { get; set; }
        [DisplayName("Transportadora")]
        public string CodigoDaTransportadora { get; set; }
        [DisplayName("Nº do Conhecimento")]
        public string NumeroDoConhecimento { get; set; }
        [DisplayName("Nº do Contrato")]
        public string NumeroDoContrato { get; set; }
        [DisplayName("Valor do Frete")]
        public decimal ValorRealDoFrete { get; set; }
        [DisplayName("Peso Total")]
        public decimal PesoTotalDaCarga { get; set; }
        public Enumeradores.StatusDoConhecimentoDeTransporte Status { get; set; }

        public string DescricaoDoStatus
        {
            get { return Status.Descricao(); }
        }
    }
}
